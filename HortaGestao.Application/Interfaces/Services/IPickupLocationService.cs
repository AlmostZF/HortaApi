using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;

namespace HortaGestao.Application.Interfaces.Services;

public interface IPickupLocationService
{
    public Task<Guid> CreateAsync(PickupLocationCreateDto pickupLocationCreateDto);
    public Task DeleteAsync(Guid id);
    public Task UpdateAsync(List<PickupLocationUpdateDto> pickupLocationUpdateDto, Guid sellerId);
    public Task<List<PickupLocationResponseDto>> GetBySellerIdAsync(Guid id);
}