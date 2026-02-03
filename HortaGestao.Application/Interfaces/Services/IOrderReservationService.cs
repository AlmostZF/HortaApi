using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;

namespace HortaGestao.Application.Interfaces.Services;

public interface IOrderReservationService
{
    Task<OrderReservationResponseDto?> GetBySecurityCodeAsync(string securityCode,Guid sellerId);
    Task<OrderReservationResponseDto> GetBySellerIdAsync(Guid id);
    Task FinishOrderAsync(Guid guid, Guid sellerId);
    Task CancelOrderAsync(string securityCode, Guid sellerId);
    Task UpdateAsync(OrderReservationUpdateDto orderReservationUpdateDto);
    Task<OrderCalculateResponseDto> CalculateForViewAsync(OrderCalculateDto orderCalculateDTO);
    Task<OrderCalculateResponseDto> CalculateForCheckoutAsync(OrderCalculateDto orderCalculateDTO);
    Task DeleteAsync(Guid id);
    Task<List<CreateOrderReservationResponseDto>> AddAsync(OrderReservationCreateDto order);
    Task<List<OrderReservationResponseDto>> GetByStatusAsync(StatusOrder status);
    Task<List<OrderReservationResponseDto>> GetAllAsync();
}