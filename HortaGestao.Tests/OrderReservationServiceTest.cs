using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.UnitOfWork;
using HortaGestao.Application.Services;
using HortaGestao.Domain.DomainService;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.IRepositories;
using Moq;

namespace HortaGestao.Tests;

public class OrderReservationServiceTest
{
    private readonly Mock<IOrderReservationRepository> _orderReservationRepository;
    private readonly Mock<IProductRepository> _productRepository;
    private readonly Mock<IPickupLocationRespository> _pickupLocationRepository;
    private readonly IReservationFeeCalculate _calculate;
    private readonly Mock<IStockRepository> _stockRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly OrderReservationService _orderReservationService;
    
    public OrderReservationServiceTest()
    {
        _orderReservationRepository = new Mock<IOrderReservationRepository>();
        _productRepository = new Mock<IProductRepository>();
        _pickupLocationRepository = new Mock<IPickupLocationRespository>();
        _calculate = new ReservationFeeCalculate();
        _stockRepository = new Mock<IStockRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _orderReservationService = new OrderReservationService(
                _orderReservationRepository.Object,
                _calculate, 
                _productRepository.Object,
                _pickupLocationRepository.Object,
                _stockRepository.Object,
                _unitOfWork.Object
            );
        
        _unitOfWork.Setup(u => u.BeginTransactionAsync())
            .Returns(Task.CompletedTask);
        
        _unitOfWork.Setup(u => u.CommitAsync())
            .Returns(Task.CompletedTask);
        
        _unitOfWork.Setup(u => u.RollbackAsync())
            .Returns(Task.CompletedTask);
    }


    [Fact]
    public async Task TestCalculateOrderForView()
    {
        var sellerId = Guid.NewGuid();
        var dto = CreateOrderCalculateDto();
        var seller = new SellerEntity("Guilherme", "11111");
        
        var listProduct = new List<ProductEntity>
        {
            CreateProductEntity(sellerId), CreateProductEntity(sellerId)
        };
        
        dto.listOrderItens.Add(CreateOrderReservationItem(sellerId, listProduct[0].Id));
        dto.listOrderItens.Add(CreateOrderReservationItem(sellerId, listProduct[1].Id));
        
        foreach (var product in listProduct)
        {
            SetProperty(product, "Seller", seller);
        }
        
        var listStockMocs = new List<StockEntity>
        {
            new StockEntity(listProduct[0].Id, 15, listProduct[0].UnitPrice),
            new StockEntity(listProduct[1].Id, 6, listProduct[1].UnitPrice)
        };

        _productRepository.Setup(repo => repo
            .GetManyProducts(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(listProduct);

        _stockRepository.Setup(repo => repo
            .GetByProductIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(listStockMocs);
        
        var result= await _orderReservationService.CalculateForViewAsync(dto);
        
        Assert.NotNull(result);
        Assert.Contains("Guilherme", result.Seller.Name);
        Assert.Equal(40, result.Total);
        Assert.Contains("Produto", result.ListOrderItens[0].Name);
        Assert.Contains("Produto", result.ListOrderItens[1].Name);
    }
    
    [Fact]
    public async Task TestCalculateOrderValidated()
    {
        var sellerId = Guid.NewGuid();
        var dto = CreateOrderCalculateDto();
        var seller = new SellerEntity("Guilherme", "11111");
        
        var listProduct = new List<ProductEntity>
        {
            CreateProductEntity(sellerId), CreateProductEntity(sellerId)
        };
        
        dto.listOrderItens.Add(CreateOrderReservationItem(sellerId, listProduct[0].Id));
        dto.listOrderItens.Add(CreateOrderReservationItem(sellerId, listProduct[1].Id));
        
        foreach (var product in listProduct)
        {
            SetProperty(product, "Seller", seller);
        }
        
        var listStockMocs = new List<StockEntity>
        {
            new StockEntity(listProduct[0].Id, 15, listProduct[0].UnitPrice),
            new StockEntity(listProduct[1].Id, 6, listProduct[1].UnitPrice)
        };

        _productRepository.Setup(repo => repo
            .GetManyProducts(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(listProduct);

        _stockRepository.Setup(repo => repo
            .GetByProductIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(listStockMocs);
        
        var result= await _orderReservationService.CalculateForCheckoutAsync(dto);
        
        Assert.NotNull(result);
        Assert.Contains("Guilherme", result.Seller.Name);
        Assert.Equal(40, result.Total);
        Assert.Equal(15, result.ListOrderItens[0].MaxQuantity);
        Assert.Equal(6, result.ListOrderItens[1].MaxQuantity);

    }
    
    [Fact]
    public async Task TestCalculateOrder_ShouldFail_WrongQuantity()
    {
        var sellerId = Guid.NewGuid();
        var dto = CreateOrderCalculateDto();
        var seller = new SellerEntity("Guilherme", "11111");
        
        var listProduct = new List<ProductEntity>
        {
            CreateProductEntity(sellerId), CreateProductEntity(sellerId)
        };
        
        dto.listOrderItens.Add(CreateOrderReservationItem(sellerId, listProduct[0].Id));
        dto.listOrderItens.Add(CreateOrderReservationItem(sellerId, listProduct[1].Id));
        
        foreach (var product in listProduct)
        {
            SetProperty(product, "Seller", seller);
        }
        
        var listStockMocs = new List<StockEntity>
        {
            new StockEntity(listProduct[0].Id, 1, listProduct[0].UnitPrice),
            new StockEntity(listProduct[1].Id, 1, listProduct[1].UnitPrice)
        };

        _productRepository.Setup(repo => repo
            .GetManyProducts(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(listProduct);

        _stockRepository.Setup(repo => repo
            .GetByProductIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(listStockMocs);
        
        var result= await _orderReservationService.CalculateForCheckoutAsync(dto);
        

        Assert.Contains("Guilherme", result.Seller.Name);
        Assert.Equal(20, result.Total);
        
        var item1 = result.ListOrderItens.FirstOrDefault(x => x.ProductId == listProduct[0].Id);
        var item2 = result.ListOrderItens.Find(x => x.ProductId == listProduct[1].Id);
        
        Assert.NotNull(item1);
        Assert.Equal(1, item1.MaxQuantity);
        Assert.Equal(1, item2.MaxQuantity);

    }
    
    private void SetProperty(object obj, string property, object value)
    {
        obj.GetType()
            .GetProperty(property)?
            .SetValue(obj, value);
    }
    
    private OrderCalculateDto CreateOrderCalculateDto()
    {
        return new OrderCalculateDto
        {
            listOrderItens = new List<OrderReservationItemDto>()
        };
    }

    private OrderReservationItemDto CreateOrderReservationItem(Guid sellerId, Guid productId)
    {
        return new OrderReservationItemDto
        {
            ProductId = productId,
            Quantity = 2,
            SellerId = sellerId,
        };
    }

    private ProductEntity CreateProductEntity(Guid sellerId, decimal? UnitPrice = 10)
    {
        
        var productMock = new ProductEntity(
            "Produto", 
            ProductType.Verduras,
            UnitPrice.Value,
            sellerId,
            "1 dia na geladeira",
            "image",
            "shortDescription",
            "largeDescription",
            "1kg");

        return productMock;
    }
    
}