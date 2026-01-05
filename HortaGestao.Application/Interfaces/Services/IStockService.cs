using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;

namespace HortaGestao.Application.Interfaces.Services;

public interface IStockService
{
    Task<List<StockResponseDto>> GetAllAsync();
    Task<StockResponseDto> GetByIdAsync(Guid productId);
    Task<StockAvailableResponseDto> GetByProductIdAsync(Guid productId);
    Task UpdateQuantityAsync(StockUpdateDto stockUpdateDto);
    Task AddAsync(StockCreateDto stockCreateDTO);

}