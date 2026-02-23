using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.UnitOfWork;
using HortaGestao.Application.Services;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.IRepositories;
using Moq;

namespace HortaGestao.Tests;

public class PickupLocationServiceTests
{

    private readonly Mock<IPickupLocationRespository> _pickupLocationRepository;
    private readonly PickupLocationService _pickupLocationService;
    private readonly Mock<ISellerRepository> _sellerRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    
    
    public PickupLocationServiceTests()
    {
        _pickupLocationRepository = new Mock<IPickupLocationRespository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _sellerRepository = new Mock<ISellerRepository>();
        _pickupLocationService = new PickupLocationService(
            _pickupLocationRepository.Object,
            _sellerRepository.Object,
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
    public async Task TestCreatePickupLocation()
    {
        var sellerId = Guid.NewGuid();

        var dto = createDto(sellerId);
        
        var seller = CreateSeller();
        SetProperty(seller, "Id", sellerId);

        _sellerRepository.Setup(repo => repo.GetByIdAsync(sellerId)).ReturnsAsync(seller);

        _pickupLocationRepository.Setup(repo => repo.CreateAsync(It.IsAny<PickupLocationEntity>()))
            .Returns(Task.CompletedTask);

        _sellerRepository.Setup(repo => repo.UpdateAsync(It.IsAny<SellerEntity>()))
            .Returns(Task.CompletedTask);

        var resultId = await _pickupLocationService.CreateAsync(dto);
        
        
        _pickupLocationRepository.Verify(repo => repo.CreateAsync(
            It.Is<PickupLocationEntity>(e => 
                e.Address.City == dto.City && 
                e.Address.CustomName == dto.CustomName &&
                e.Address.ZipCode == dto.ZipCode &&
                e.SellerEntityId == sellerId)), Times.Once);
        
        _sellerRepository.Verify(repo => repo.UpdateAsync(
            It.Is<SellerEntity>(s => s.PickupLocations.FirstOrDefault().Id == resultId)),
            Times.Once);
        
        Assert.NotEqual(Guid.Empty, resultId);

    }

    private SellerEntity CreateSeller()
    {
        return new SellerEntity("Guilherme", "111");
    }

    private PickupLocationCreateDto createDto(Guid sellerId)
    {
        return new PickupLocationCreateDto
        {
            City = "city",
            CustomName = "Custom Name",
            Neighborhood = "Neighborhood",
            Number = "1",
            PickupDays = new List<DayOfWeek> { DayOfWeek.Friday, DayOfWeek.Monday },
            SellerId = sellerId,
            State = "State",
            Street = "Street",
            ZipCode = "ZipCode",
        };
    }

    private void SetProperty(object obj, string property, object value)
    {
        obj.GetType()
            .GetProperty(property)?
            .SetValue(obj, value);
    }
}