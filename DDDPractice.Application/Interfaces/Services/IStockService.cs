using DDDPractice.DDDPractice.Domain.Entities;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;

namespace DDDPractice.Application.Interfaces;

public interface IStockService
{
    Task<List<StockResponseDto>> GetAllAsync();
    Task<StockResponseDto> GetByIdAsync(Guid productId);
    Task<StockAvailableResponseDto> GetByProductIdAsync(Guid productId);
    Task UpdateQuantityAsync(StockUpdateDTO stockUpdateDto);
    Task AddAsync(StockCreateDTO stockCreateDTO);

}