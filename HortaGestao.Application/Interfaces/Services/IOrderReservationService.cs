using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;

namespace HortaGestao.Application.Interfaces.Services;

public interface IOrderReservationService
{
    Task<OrderReservationResponseDto?> GetBySecurityCodeAsync(string securityCode,Guid sellerId);
    Task<OrderReservationResponseDto> GetBySellerIdAsync(Guid id);
    Task FinishOrder(Guid guid, Guid sellerId);
    Task UpdateAsync(OrderReservationUpdateDto orderReservationUpdateDto);
    Task<OrderCalculateResponseDto> CalculateAsync(OrderCalculateDto orderCalculateDTO);
    Task DeleteAsync(Guid id);
    Task<List<CreateOrderReservationResponseDto>> AddAsync(OrderReservationCreateDto order);
    Task<List<OrderReservationResponseDto>> GetByStatusAsync(StatusOrder status);
    Task<List<OrderReservationResponseDto>> GetAllAsync();
}