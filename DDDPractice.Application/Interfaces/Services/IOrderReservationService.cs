using DDDPractice.DDDPractice.Domain.Enums;
using DDDPractice.DDDPractice.Domain.ValueObjects;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;

namespace DDDPractice.Application.Interfaces;

public interface IOrderReservationService
{
    Task<List<OrderReservationResponseDto>> GetBySecurityCodeAsync(string securityCode);
    Task<OrderReservationResponseDto> GetByIdAsync(Guid id);
    Task UpdateStatusAsync(OrderReservationResponseDto orderReservationResponseDto);
    Task UpdateAsync(OrderReservationUpdateDTO orderReservationUpdateDto);
    Task<OrderCalculateResponseDto> CalculateAsync(OrderCalculateDTO orderCalculateDTO);
    Task DeleteAsync(Guid id);
    Task AddAsync(OrderReservationCreateDTO order);
    Task<List<OrderReservationResponseDto>> GetByStatusAsync(StatusOrder status);
    Task<List<OrderReservationResponseDto>> GetAllAsync();
}