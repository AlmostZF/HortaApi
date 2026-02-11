using HortaGestao.Application.DTOs.Request;
using Moq;
using HortaGestao.Application.Services;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.IRepositories;
using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Tests;

public class SellerServiceTests
{
    private readonly Mock<ISellerRepository> _RepositoryMock;
    private readonly SellerService _sellerService;
    
    public SellerServiceTests()
    {
        _RepositoryMock = new Mock<ISellerRepository>();
        _sellerService = new SellerService(_RepositoryMock.Object);
    }
    
    [Fact]
    public async Task TestGetSeller()
    {
        var sellerId = Guid.NewGuid();
        var mockSeller = new SellerEntity("Guilherme", "(31)99999-9999");
        
        var pickupLocation = MockPicupLocation(sellerId);
        
        mockSeller.AddPickupLocation(pickupLocation);
        
        typeof(SellerEntity)
            .GetProperty("Id")?
            .SetValue(mockSeller, sellerId);
        
        _RepositoryMock.Setup(
            repo => repo.GetByIdAsync(sellerId)).ReturnsAsync(mockSeller);
        
        var result = await _sellerService.GetByIdAsync(sellerId);
    
        Assert.NotNull(result);
        Assert.Equal(sellerId, result.Id);
        Assert.Single(result.ListPickupLocations);
        _RepositoryMock.Verify(repo=> repo.GetByIdAsync(sellerId), Times.Once);
    }
    
    [Fact]
    public async Task TestGetAllSellers()
    {
        var listMockSellers = new List<SellerEntity> {
            new SellerEntity("Guilherme", "1111"),
            new SellerEntity("João", "2222")
        };
        foreach (var seller in listMockSellers)
        {
            var pickupLocation = MockPicupLocation(seller.Id);
            seller.AddPickupLocation(pickupLocation);
        }
        
        _RepositoryMock.Setup(repo=> repo.GetAllAsync()).ReturnsAsync(listMockSellers);
        
        var result = await _sellerService.GetAllAsync();
        
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Collection(result, 
            item1 => {
                Assert.Equal("Guilherme", item1.Name);
                Assert.Equal("street",  item1.ListPickupLocations[0].Street);
            },
            item2 => {
                Assert.Equal("João", item2.Name);
                Assert.Equal("street",  item2.ListPickupLocations[0].Street);
            }
        );
        
        _RepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task TestUpdateSeller()
    {
        var sellerId = Guid.NewGuid();
        var mockSeller = new SellerEntity("Guilherme", "1111");
        
        var pickupLocation = MockPicupLocation(sellerId);
        mockSeller.AddPickupLocation(pickupLocation);
        
        var updateDto = new SellerUpdateDto
        {
            Id = sellerId,
            Name = "Guilherme Alterado",
            PhoneNumber = "2222",
        }; 
        
        _RepositoryMock.Setup(repo => repo.GetByIdAsync(sellerId)).ReturnsAsync(mockSeller);

        await _sellerService.UpdateAsync(updateDto);
        
        Assert.Equal("Guilherme Alterado", mockSeller.Name);
        Assert.Equal("2222", mockSeller.PhoneNumber);
        
        _RepositoryMock.Verify(repo => repo.UpdateAsync(mockSeller), Times.Once);
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