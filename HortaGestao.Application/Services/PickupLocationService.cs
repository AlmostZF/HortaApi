using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Mappers;
using HortaGestao.Domain.IRepositories;

namespace HortaGestao.Application.Services;

public class PickupLocationService : IPickupLocationService
{   
    private readonly IPickupLocationRespository _pickupLocationRespository;
    private readonly ISellerRepository _sellerRepository;
    
    public PickupLocationService(IPickupLocationRespository pickupLocationRespository,
        ISellerRepository sellerRepository)
    {
        _pickupLocationRespository = pickupLocationRespository;
        _sellerRepository = sellerRepository;
    }
    
    public async Task<Guid> CreateAsync(PickupLocationCreateDto pickupLocationCreateDto)
    {
        
        var seller = await _sellerRepository.GetByIdAsync(pickupLocationCreateDto.SellerId);
        if (seller == null)
            throw new Exception("Seller not found.");

        var pickupLocationEntity = PickupLocationMapper.ToEntity(pickupLocationCreateDto);
        await _pickupLocationRespository.CreateAsync(pickupLocationEntity);
        
        seller.AddPickupLocation(pickupLocationEntity);
        
        await _sellerRepository.UpdateAsync(seller);
        
        return pickupLocationEntity.Id; 
    }

    public async Task DeleteAsync(Guid id)
    {
        await _pickupLocationRespository.DeleteAsync(id);
    }
    

    public async Task UpdateAsync(PickupLocationUpdateDto pickupLocationUpdateDto)
    {
        var existingPickupLocation = await _pickupLocationRespository.GetByIdAsync(pickupLocationUpdateDto.Id);
        
        PickupLocationMapper.ToUpdateEntity(existingPickupLocation, pickupLocationUpdateDto);
        
        await _pickupLocationRespository.UpdateAsync(existingPickupLocation);
    }

    public async Task<List<PickupLocationResponseDto>> GetBySellerIdAsync(Guid id)
    {
        var pickupLocation = await _pickupLocationRespository.GetBySellerIdAsync(id);
        var pickupLocationList = PickupLocationMapper.ToDtoList(pickupLocation);
        return pickupLocationList;
    }
    
    
}