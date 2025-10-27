using DDD_Practice.DDDPractice.Domain.Enums;
using DDD_Practice.DDDPractice.Domain.ValueObjects;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.Services;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.OrderReservation;

public class GetOrderBySecurityCodeUseCase
{
    private readonly OrderReservationService _orderReservationService;

    public GetOrderBySecurityCodeUseCase(OrderReservationService orderReservationService)
    {
        _orderReservationService = orderReservationService;
    }

    public async Task<Result<List<OrderReservationResponseDto>>> ExecuteAsync(string securityCode)
    {
        try
        {
            var orderReservationDtos = await _orderReservationService.GetBySecurityCodeAsync(securityCode);
            return Result<List<OrderReservationResponseDto>>.Success(orderReservationDtos,200);
        }
        catch (Exception e)
        {
            return Result<List<OrderReservationResponseDto>>.Failure("Erro ao buscar lista de compra", 500);
        }
    }
}