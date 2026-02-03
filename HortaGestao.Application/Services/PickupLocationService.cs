using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Interfaces.UnitOfWork;
using HortaGestao.Application.Mappers;
using HortaGestao.Domain.IRepositories;

namespace HortaGestao.Application.Services;

public class PickupLocationService : IPickupLocationService
{   
    private readonly IPickupLocationRespository _pickupLocationRespository;
    private readonly ISellerRepository _sellerRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public PickupLocationService(IPickupLocationRespository pickupLocationRespository,
        ISellerRepository sellerRepository,
        IUnitOfWork unitOfWork)
    {
        _pickupLocationRespository = pickupLocationRespository;
        _sellerRepository = sellerRepository;
        _unitOfWork = unitOfWork;
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
    

    public async Task UpdateAsync(List<PickupLocationUpdateDto> pickupLocationUpdateDto, Guid sellerId)
    {
        
        var currentLocations = await _pickupLocationRespository.GetBySellerIdAsync(sellerId);
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var Ids = pickupLocationUpdateDto.Select(x => x.Id).ToList();
            var toDelete = currentLocations.Where(x => !Ids.Contains(x.Id)).ToList();
            foreach (var item in toDelete) await _pickupLocationRespository.DeleteAsync(item.Id);
            
            foreach (var pickupLocantion in pickupLocationUpdateDto)
            {
                var existingPickupLocation = currentLocations.FirstOrDefault(x => x.Id == pickupLocantion.Id);
                if (existingPickupLocation != null)
                {
                    PickupLocationMapper.ToUpdateEntity(existingPickupLocation, pickupLocantion);
                    await _pickupLocationRespository.UpdateAsync(existingPickupLocation);
                }
                else
                {       
                    var createPickupLocation = new PickupLocationCreateDto
                    {
                        City = pickupLocantion.City,
                        Neighborhood = pickupLocantion.Neighborhood,
                        Number = pickupLocantion.Number,
                        Street = pickupLocantion.Street,
                        ZipCode = pickupLocantion.ZipCode,
                        State = pickupLocantion.State,
                        PickupDays = pickupLocantion.PickupDays,
                        CustomName = pickupLocantion.CustomName,
                        SellerId = sellerId
                    };
                    
                     var newPickupLocation = PickupLocationMapper.ToCreateEntity(createPickupLocation);
                     await _pickupLocationRespository.CreateAsync(newPickupLocation);
                }
                
                

            }
                
            await _unitOfWork.CommitAsync();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
        
    }

    public async Task<List<PickupLocationResponseDto>> GetBySellerIdAsync(Guid id)
    {
        var pickupLocation = await _pickupLocationRespository.GetBySellerIdAsync(id);
        var pickupLocationList = PickupLocationMapper.ToDtoList(pickupLocation);
        return pickupLocationList;
    }
    
    
}