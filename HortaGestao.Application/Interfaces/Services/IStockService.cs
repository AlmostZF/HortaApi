using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;

namespace HortaGestao.Application.Interfaces.Services;

public interface IStockService
{
    Task<List<StockResponseDto>> GetAllAsync(Guid sellerId);
    Task<StockResponseDto> GetByIdAsync(Guid stockId, Guid sellerId);
    Task<StockAvailableResponseDto> GetByProductIdAsync(Guid productId, Guid sellerId);
    Task UpdateQuantityAsync(StockUpdateDto stockUpdateDto, Guid sellerId);
    Task AddAsync(StockCreateDto stockCreateDTO, Guid sellerId);

}