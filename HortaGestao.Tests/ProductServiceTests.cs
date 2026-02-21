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
        var dto = CreateProductDto("produto", "fruta"); 
        
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
        
        var sellerMock = new SellerEntity("Guilherme", "2222");
        SetProperty(sellerMock, "Id", sellerId);
        
        var productMoc = CreateProductEntity(sellerId);
        SetProperty(productMoc, "Id", productId);
        SetProperty(productMoc, "Seller", sellerMock);
        SetProperty(productMoc, "IsActive", true);

        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productMoc.Id)).ReturnsAsync(productMoc);

        var result = await _productService.GetByIdAsync(productId);
        
        Assert.NotNull(result);
        Assert.Equal("Produto", result.Name);

        _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);

    }
    
    [Fact]
    public async Task TestGetInativeProductById_ShouldReturnNull()
    {
        var sellerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        
        var sellerMock = new SellerEntity("Guilherme", "2222");
        SetProperty(sellerMock, "Id", sellerId);
        
        var productMoc = CreateProductEntity(sellerId);
        SetProperty(productMoc, "Id", productId);
        SetProperty(productMoc, "Seller", sellerMock);
        SetProperty(productMoc, "IsActive", false);

        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productMoc.Id)).ReturnsAsync(productMoc);

        var result = await _productService.GetByIdAsync(productId);
        
        Assert.Null(result);
        
        _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);

    }

    [Fact]
    public async Task TestFiterProducts()
    {
        var sellerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var dto = new ProductFilterDto
        {
            Category = "Verduras",
            Name = "Produto"
        };

        var sellerMoc = new SellerEntity("Guilherme", "2222");
        SetProperty(sellerMoc, "Id", sellerId);

        var productMoc = CreateProductEntity(sellerId);
        SetProperty(productMoc, "Seller", sellerMoc);
        SetProperty(productMoc, "Id", productId);
        SetProperty(productMoc, "IsActive", true);
        
        _productRepositoryMock.Setup(repo => repo.FilterAsync(It.Is<ProductFilter>(f => 
            f.Category == "Verduras" && 
            f.Name == "Produto"
        )))
        .ReturnsAsync(new List<ProductEntity> { productMoc });
        
        _productRepositoryMock.Setup(repo => repo.CountAsync(It.IsAny<ProductFilter>()))
           .ReturnsAsync(1); 

        var result = await _productService.FilterAsync(dto);

        Assert.NotNull(result.Data);
        Assert.Single(result.Data); 
        Assert.Equal("Verduras", result.Data.First().ProductType); 
        
        _productRepositoryMock.Verify(repo => repo.FilterAsync(It.IsAny<ProductFilter>()), Times.Once);
    }

    [Fact]
    public async Task TestFilterProduct_ShouldReturnEmpty()
    {
        var sellerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var dto = new ProductFilterDto
        {
            Category = "Frutas"
        };
        
        var sellerMoc = new SellerEntity("Guilherme", "2222");
        SetProperty(sellerMoc, "Id", sellerId);

        var productMoc = CreateProductEntity(sellerId);
        SetProperty(productMoc, "Id", productId);
        SetProperty(productMoc, "SellerId", sellerId);

        _productRepositoryMock.Setup(repo => repo.FilterAsync(It.IsAny<ProductFilter>()))
            .ReturnsAsync(new List<ProductEntity>());
        
        _productRepositoryMock.Setup(repo => repo.CountAsync(It.IsAny<ProductFilter>()))
            .ReturnsAsync(0);

        var result = await _productService.FilterAsync(dto);

        Assert.Empty(result.Data);
        Assert.Equal(0, result.Pagination.TotalItems);

    }


    private ProductCreateDto CreateProductDto(string name, string type)
    {
        var productType = new ProductType();
        
        var productMock = new ProductCreateDto
        {
            ConservationDays = "1 dia",
            LargeDescription = "LargeDescription",
            Name = name,
            ProductType = type,
            ShortDescription = "ShortDescription",
            UnitPrice = 2,
            Weight = "1"
        };

        return productMock;
    }
    
    private ProductEntity CreateProductEntity(Guid sellerId)
    {
        
        var productMock = new ProductEntity(
            "Produto", 
            ProductType.Verduras,
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