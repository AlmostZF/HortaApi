using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;

namespace HortaGestao.Application.Interfaces.Services;

public interface ISellerService
{
    Task<SellerResponseDto> GetByIdAsync(Guid id);
    Task<Guid> AddAsync(SellerCreateDto sellerCreate);
    Task UpdateAsync(SellerUpdateDto sellerUpdate);
    Task DeleteAsync(Guid id);
    Task<List<SellerResponseDto>> GetAllAsync();
}