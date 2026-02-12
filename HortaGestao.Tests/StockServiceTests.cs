using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Services;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.IRepositories;
using HortaGestao.Domain.ValueObjects;
using Moq;

namespace HortaGestao.Tests;

public class StockServiceTests
{
    private readonly Mock<IStockRepository> _stockRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly StockService _stockService;
    private readonly ProductService _productService;


    public StockServiceTests()
    {
        _stockRepositoryMock = new Mock<IStockRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _stockService = new StockService(_stockRepositoryMock.Object, _productRepositoryMock.Object);
    }

    [Fact]
    public async Task TestCreateStock()
    {
        var sellerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var productMock = CreateProductEntity(sellerId);
        
        var dto = new StockCreateDto()
        {
            ProductId = productId,
            Quantity = 4,
        };
        
        _stockRepositoryMock.Setup(repo => repo.GetByProductIdAsync(productId, sellerId))
            .ReturnsAsync((StockEntity)null);
        
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync(productMock);

        await _stockService.AddAsync(dto, sellerId);

        _stockRepositoryMock.Verify(repo => repo.AddAsync(
            It.Is<StockEntity>(s => s.Total == 4)), Times.Once);

    }
    
    [Fact]
    public async Task TestUpdateStock()
    {
        var sellerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var stockId = Guid.NewGuid();
        
        var stockMock = new StockEntity(productId, 0, 0);
        SetProperty(stockMock, "Product", stockMock);

        var dto = new StockUpdateDto()
        {
            Quantity = 7,
            Id = stockId
        };
        
        _stockRepositoryMock.Setup(repo => repo.GetByIdAsync(dto.Id, sellerId))
            .ReturnsAsync(stockMock);
        
        await _stockService.UpdateQuantityAsync(dto, sellerId);

        _stockRepositoryMock.Verify(repo => repo.UpdateQuantityAsync(
            It.Is<StockEntity>(s => s.Total == 7)), Times.Once);

    }

    [Fact]
    public async Task TestGetStockByProduct()
    {
        var sellerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var pickupLocation = MockPicupLocation(sellerId);

        var sellerMock = new SellerEntity("Guilherme", "2222");
        SetProperty(sellerMock, "Id", sellerId);

        sellerMock.AddPickupLocation(pickupLocation);
        
        var productMoc = CreateProductEntity(sellerId);
        SetProperty(productMoc, "Seller", sellerMock);
        
        var stockMoc = new StockEntity(productId, 0, 0);
        SetProperty(stockMoc, "Product", productMoc);


        _stockRepositoryMock.Setup(repo => repo.GetByProductIdAsync(productMoc.Id, sellerId))
            .ReturnsAsync(stockMoc);

        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(sellerId));

        var result = await _stockService.GetByProductIdAsync(productMoc.Id, sellerId);

        Assert.NotNull(result);
        Assert.NotNull(result.Product);
        Assert.Equal("Produto", result.Product.Name);
        Assert.Equal(0, result.StockLimit);

        _stockRepositoryMock.Verify(repo => repo.GetByProductIdAsync(productMoc.Id, sellerMock.Id), Times.Once);
    }

    private ProductEntity CreateProductEntity(Guid sellerId)
    {
        var productType = new ProductType();
        
        var productMock = new ProductEntity(
            "Produto",
            productType,
            1,
            sellerId,
            "1 dia na geladeira",
            "image",
            "shortDescription",
            "largeDescription",
            "1kg");

        return productMock;
    }

    private void SetProperty(object obj, string property, object value)
    {
        obj.GetType()
            .GetProperty(property)?
            .SetValue(obj, value);
    }

    private PickupLocationEntity MockPicupLocation(Guid sellerId)
    {
        var addresMock = new Address("street", "number", "city", "zipCode", "state",
            "neighborhood", "customName");
        var pickupDays = new List<PickupDay>
        {
            new PickupDay(DayOfWeek.Monday),
            new PickupDay(DayOfWeek.Tuesday),
        };
        
        var mockPickupLocation = new PickupLocationEntity(addresMock, pickupDays, sellerId);
        return mockPickupLocation;
    }
}