using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Domain.Entities;
using HortaGestao.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace HortaGestao.Application.Interfaces.Services;

public interface IStockService
{
    Task<List<StockResponseDto>> GetAllAsync(Guid sellerId);
    Task<StockResponseDto> GetByIdAsync(Guid stockId, Guid sellerId);
    Task<StockAvailableResponseDto> GetByProductIdAsync(Guid productId, Guid sellerId);
    Task UpdateQuantityAsync(StockUpdateDto stockUpdateDto, Guid sellerId);
    Task CreateAsync(StockCreateDto stockCreateDTO, Guid sellerId);
    Task DebitStockAsync(List<OrderReservationItemDto> listOrderItens, IEnumerable<StockEntity> listStock);
    Task AddStockAsync(OrderReservationEntity orderReservation, IEnumerable<StockEntity> listStock);
    Task<MessagingLogDto> CreateBulkStockAsync(ImportMessagingDto importData, IHubContext<ImportHub> hubContext,
        Guid sellerId);

}