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
    public async Task TestGetInactiveProductById_ShouldReturnNull()
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
    public async Task TestFilterProducts()
    {
        var sellerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var dto = CreateFilterDto("Verduras");

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

        var dto = CreateFilterDto();
        
        var sellerMoc = new SellerEntity("Guilherme", "2222");
        SetProperty(sellerMoc, "Id", sellerId);

        var productMoc = CreateProductEntity(sellerId);
        SetProperty(productMoc, "Seller", sellerMoc);
        SetProperty(productMoc, "Id", productId);
        SetProperty(productMoc, "IsActive", true);

        _productRepositoryMock.Setup(repo => repo.FilterAsync(It.IsAny<ProductFilter>()))
            .ReturnsAsync(new List<ProductEntity>());
        
        _productRepositoryMock.Setup(repo => repo.CountAsync(It.IsAny<ProductFilter>()))
            .ReturnsAsync(0);

        var result = await _productService.FilterAsync(dto);

        Assert.Empty(result.Data);
        Assert.Equal(0, result.Pagination.TotalItems);

    }

    [Fact]
    public async Task TestUpdateProduct()
    {
        var productId = Guid.NewGuid();
        var sellerId = Guid.NewGuid();

        var dto = CreateProductUpdateDto(productId);

        var productMoc = CreateProductEntity(sellerId);
        SetProperty(productMoc, "Id", productId);
        
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync(productMoc);

        _productRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<ProductEntity>()))
            .Returns(Task.CompletedTask);

        await _productService.UpdateAsync(dto, sellerId);

        _productRepositoryMock.Verify(repo => repo.UpdateAsync(
            It.Is<ProductEntity>(p => p.Id == productId && 
                                      p.Name == dto.Name && 
                                      p.UnitPrice == dto.UnitPrice &&
                                      p.SellerId == sellerId
            )));
    }

    [Fact]
    public async Task TestUpdateProductStatus()
    {
        var productId = Guid.NewGuid();
        var sellerId = Guid.NewGuid();

        var dto = CreateProductUpdateStatusDto(productId, true);

        var productMoc = CreateProductEntity(sellerId);
        SetProperty(productMoc, "Id", productId);

        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(productMoc);

        _productRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<ProductEntity>()))
            .Returns(Task.CompletedTask);
        
        await _productService.UpdateStatusAsync(dto);

        _productRepositoryMock.Verify(repo => repo.UpdateAsync(
            It.Is<ProductEntity>(p => p.IsActive == true)));
    }

    private ProductUpdateStatusDto CreateProductUpdateStatusDto(Guid productId, bool isActive)
    {
        return new ProductUpdateStatusDto
        {
            IsActive = isActive,
            Id = productId
        };
    }

    private ProductUpdateDto CreateProductUpdateDto(Guid productId)
    {
        return new ProductUpdateDto
        {
            Id = productId,
            UnitPrice = 5,
            Name = "Produto Editado"
        };
    }

    private ProductFilterDto CreateFilterDto(string? category = "Frutas")
    {
        return new ProductFilterDto {
            Category = category,
            Name = "Produto"
        };
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
    
}