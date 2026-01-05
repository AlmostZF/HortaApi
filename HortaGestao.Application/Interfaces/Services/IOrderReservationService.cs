using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;

namespace HortaGestao.Application.Interfaces.Services;

public interface IOrderReservationService
{
    Task<List<OrderReservationResponseDto>> GetBySecurityCodeAsync(string securityCode);
    Task<OrderReservationResponseDto> GetByIdAsync(Guid id);
    Task UpdateStatusAsync(OrderReservationResponseDto orderReservationResponseDto);
    Task UpdateAsync(OrderReservationUpdateDto orderReservationUpdateDto);
    Task<OrderCalculateResponseDto> CalculateAsync(OrderCalculateDto orderCalculateDTO);
    Task DeleteAsync(Guid id);
    Task AddAsync(OrderReservationCreateDto order);
    Task<List<OrderReservationResponseDto>> GetByStatusAsync(StatusOrder status);
    Task<List<OrderReservationResponseDto>> GetAllAsync();
}