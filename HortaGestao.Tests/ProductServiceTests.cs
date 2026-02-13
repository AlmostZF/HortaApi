using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Services;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.IRepositories;
using HortaGestao.Domain.ValueObjects;
using Moq;

namespace HortaGestao.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IStorageService> _storageServiceMock;
    private readonly ProductService _productService;
    
    
    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _storageServiceMock = new Mock<IStorageService>();
        _productService = new ProductService(_productRepositoryMock.Object, _storageServiceMock.Object);
    }

    [Fact]
    public async Task TestCreateProduct()
    {
        var sellerId = Guid.NewGuid();
        var dto = CreateProductDto(); 
        
        await _productService.AddAsync(dto, sellerId);
        
        _productRepositoryMock.Verify(repo => repo.AddAsync(
                It.Is<ProductEntity>(p => p.Name == "produto" && p.SellerId == sellerId)), 
            Times.Once);
    }


    [Fact]
    public async Task TestGetProductById()
    {
        var sellerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var pickupLocation = MockPicupLocation(sellerId);
        
        var sellerMock = new SellerEntity("Guilherme", "2222");
        SetProperty(sellerMock, "Id", sellerId);
        
        sellerMock.AddPickupLocation(pickupLocation);
        
        var productMoc = CreateProductEntity(sellerId);
        SetProperty(productMoc, "Seller", sellerMock);
        SetProperty(productMoc, "Id", productId);

        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(sellerId)).ReturnsAsync(productMoc);

        var result = await _productService.GetByIdAsync(sellerId);

        Assert.NotNull(result);
        Assert.Equal("produto", result.Name);

    }


    private ProductCreateDto CreateProductDto()
    {
        var productType = new ProductType();
        
        var productMock = new ProductCreateDto
        {
            ConservationDays = "1 dia",
            LargeDescription = "LargeDescription",
            Name = "produto",
            ProductType = "Pendente",
            ShortDescription = "ShortDescription",
            UnitPrice = 2,
            Weight = "1"
        };

        return productMock;
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