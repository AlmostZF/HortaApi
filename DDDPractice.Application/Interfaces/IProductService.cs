using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.Interfaces;

public interface IProductService
{
    Task<ProductResponseDto> GetByIdAsync(Guid id);
    Task UpdateAsync (ProductUpdateDTO productUpdate);
    Task DeleteAsync(Guid id);
    Task<Guid> AddAsync(ProductCreateDTO productCreateDTO);
    Task<List<ProductResponseDto>> GetAllAsync();
    Task<PagedResponse<ProductResponseDto>> FilterAsync(ProductFilterDTO productFilterDto);
}