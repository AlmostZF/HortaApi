using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.Interfaces.Services;

public interface IProductService
{
    Task<ProductResponseDto> GetByIdAsync(Guid id);
    Task UpdateAsync (ProductUpdateDto productUpdate);
    Task DeleteAsync(Guid id);
    Task<Guid> AddAsync(ProductCreateDto productCreateDTO);
    Task<List<ProductResponseDto>> GetAllAsync();
    Task<PagedResponse<ProductResponseDto>> FilterAsync(ProductFilterDto productFilterDto);
}