using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.UnitOfWork;
using HortaGestao.Application.Services;
using HortaGestao.Domain.DomainService;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.IRepositories;
using HortaGestao.Domain.ValueObjects;
using Moq;

namespace HortaGestao.Tests;

public class OrderReservationServiceTests
{
    private readonly Mock<IOrderReservationRepository> _orderReservationRepository;
    private readonly Mock<IProductRepository> _productRepository;
    private readonly Mock<IPickupLocationRespository> _pickupLocationRepository;
    private readonly IReservationFeeCalculate _calculate;
    private readonly Mock<IStockRepository> _stockRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly OrderReservationService _orderReservationService;
    
    public OrderReservationServiceTests()
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

    [Fact]
    public async Task TestCancelOrder()
    {
        string securityCode = "ABC1";
        var sellerId = Guid.NewGuid();
        var pickupLocation = MockPicupLocation(sellerId);
        
        var orderId = Guid.NewGuid();
        var order = MockOrderReservation(pickupLocation, sellerId);
        SetProperty(order, "Id", orderId);
        
        var listProduct = new List<ProductEntity>
        {
            CreateProductEntity(sellerId), CreateProductEntity(sellerId)
        };
        
        var listStockMocs = new List<StockEntity>
        {
            new StockEntity(listProduct[0].Id, 1, listProduct[0].UnitPrice),
            new StockEntity(listProduct[1].Id, 1, listProduct[1].UnitPrice)
        };
        
        _orderReservationRepository.Setup(repo => repo
            .GetBySecurityCodeAsync(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(order);

        _stockRepository.Setup(repo => repo
            .GetByProductIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(listStockMocs);

        await _orderReservationService.CancelOrderAsync(securityCode, sellerId);

        _stockRepository.Verify(repo => repo
            .UpdateRangeAsync(It.Is<IEnumerable<StockEntity>>(
                s=> s.FirstOrDefault().Quantity == 1)),Times.Once);

        _orderReservationRepository.Verify(repo => repo
            .UpdateStatusAsync(It.Is<string>(s => s == "Cancelada"),
                It.Is<Guid>(id => id == order.Id)), Times.Once);
        
        _unitOfWork.Verify(work => work.BeginTransactionAsync(),Times.Once);
        _unitOfWork.Verify(work => work.RollbackAsync(),Times.Never);
        _unitOfWork.Verify(work => work.CommitAsync(),Times.Once);
    }

    [Fact]
    public async Task TestCancelOrder_ShouldRollback()
    {
        string securityCode = "ABC1";
        var sellerId = Guid.NewGuid();

        _orderReservationRepository.Setup(repo => repo
                .GetBySecurityCodeAsync(It.IsAny<string>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception("Reserva não encontrada."));

        var exception =
            await Assert.ThrowsAsync<Exception>(() =>
                _orderReservationService.CancelOrderAsync(securityCode, sellerId));

        Assert.Equal("Reserva não encontrada.", exception.Message);

        _unitOfWork.Verify(work => work.BeginTransactionAsync(), Times.Once);
        _unitOfWork.Verify(work => work.RollbackAsync(), Times.Once);
        _unitOfWork.Verify(work => work.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task TestFinishOrder()
    {
        var sellerId = Guid.NewGuid();
        var pickupLocation = MockPicupLocation(sellerId);
        var orderId = Guid.NewGuid();
        var order = MockOrderReservation(pickupLocation, sellerId);
        SetProperty(order, "Id", orderId);

        _orderReservationRepository.Setup(repo => repo.GetByIdAsync(
            It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(order);

        await _orderReservationService.FinishOrderAsync(orderId, sellerId);

        _orderReservationRepository.Verify(repo => repo
            .UpdateStatusAsync(It.Is<string>(s => s == "Confirmada"), It.IsAny<Guid>()),Times.Once);
    }
    
    
    [Fact]
    public async Task TestGetStatus()
    {
        var sellerId = Guid.NewGuid();
        var status = StatusOrder.Pendente;
        var pickupLocation = MockPicupLocation(sellerId);
        var orderId = Guid.NewGuid();
        var order = MockOrderReservation(pickupLocation, sellerId);
        SetProperty(order, "Id", orderId);

        _orderReservationRepository.Setup(repo => repo
            .GetByStatusAsync(It.IsAny<StatusOrder>())).ReturnsAsync(new List<OrderReservationEntity> { order });
        
        var result = await _orderReservationService.GetByStatusAsync(status);

        Assert.NotEmpty(result);
        Assert.Equal("Pendente", result[0].OrderStatus);

    }
    
    [Fact]
    public async Task TestGetStatus_ShouldRetunsEmpty_WhenNoOrdersMatchStatus()
    {
        var status = StatusOrder.Cancelada;

        _orderReservationRepository.Setup(repo => repo
            .GetByStatusAsync(It.IsAny<StatusOrder>())).ReturnsAsync(new List<OrderReservationEntity> ());
        
        var result = await _orderReservationService.GetByStatusAsync(status);
        
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task TestGetBySecurityCode_ShouldRetunsEmpty_WhenNoSecurityCodeMatchStatus()
    {
        var securityCode = new SecurityCode("ABC1");
        var sellerId = Guid.NewGuid();
        var status = StatusOrder.Cancelada;
        var pickupLocation = MockPicupLocation(sellerId);
        var orderId = Guid.NewGuid();
        var order = MockOrderReservation(pickupLocation, sellerId);
        SetProperty(order, "Id", orderId);
        SetProperty(order, "SecurityCode", securityCode);

        _orderReservationRepository.Setup(repo => repo
            .GetBySecurityCodeAsync(
                It.IsAny<string>(),
                It.IsAny<Guid>())).ReturnsAsync(order);
        
        var result = await _orderReservationService.GetBySecurityCodeAsync(
            securityCode.Value, sellerId);
        
        Assert.NotNull(result);
        Assert.NotEqual("ABC2", result.UserResponse.SecurityCode);
    }
    
    [Fact]
    public async Task TestGetBySecurityCode()
    {
        var securityCode = new SecurityCode("ABC1");
        var sellerId = Guid.NewGuid();
        var status = StatusOrder.Cancelada;
        var pickupLocation = MockPicupLocation(sellerId);
        var orderId = Guid.NewGuid();
        var order = MockOrderReservation(pickupLocation, sellerId);
        SetProperty(order, "Id", orderId);
        SetProperty(order, "SecurityCode", securityCode);

        _orderReservationRepository.Setup(repo => repo
            .GetBySecurityCodeAsync(
                It.IsAny<string>(),
                It.IsAny<Guid>())).ReturnsAsync(order);
        
        var result = await _orderReservationService.GetBySecurityCodeAsync(
            securityCode.Value, sellerId);
        
        Assert.NotNull(result);
        Assert.Equal("ABC1", result.UserResponse.SecurityCode);
    }
    


    
    private OrderReservationEntity MockOrderReservation(PickupLocationEntity location, Guid sellerId)
    {
        var pickupDate = new DateTime();
        var deadline = new DateTime();
        return OrderReservationEntity.CreateForGuest(
            "Guilherme",
            "Guilherme@teste.com", "111",
            location,
            pickupDate,
            deadline,
            0,
            sellerId);
    }

    
    private PickupLocationEntity MockPicupLocation(Guid sellerId, string? customName = "customName")
    {
        var addresMock = new Address("street", "number", "city", "zipCode", "state",
            "neighborhood", customName);
        var pickupDays = new List<PickupDay>
        {
            new PickupDay(DayOfWeek.Monday),
            new PickupDay(DayOfWeek.Tuesday),
        };
        
        var mockPickupLocation = new PickupLocationEntity(addresMock, pickupDays, sellerId);
        return mockPickupLocation;
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