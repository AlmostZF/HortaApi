using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;

namespace DDDPractice.Application.Interfaces;

public interface ISellerService
{
    Task<SellerResponseDto> GetByIdAsync(Guid id);
    Task<Guid> AddAsync(SellerCreateDTO sellerCreate);
    Task UpdateAsync(SellerUpdateDTO sellerUpdate);
    Task DeleteAsync(Guid id);
    Task<List<SellerResponseDto>> GetAllAsync();
}