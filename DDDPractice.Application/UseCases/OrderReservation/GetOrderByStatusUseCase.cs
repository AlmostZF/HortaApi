using DDDPractice.DDDPractice.Domain.Enums;
using DDDPractice.DDDPractice.Domain.ValueObjects;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.Services;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.OrderReservation;

public class GetOrderByStatusUseCase
{
    private readonly OrderReservationService _orderReservationService;

    public GetOrderByStatusUseCase(OrderReservationService orderReservationService)
    {
        _orderReservationService = orderReservationService;
    }

    public async Task<Result<List<OrderReservationResponseDto>>> ExecuteAsync(StatusOrder securityCode)
    {
        try
        {
            var orderReservationDtos = await _orderReservationService.GetByStatusAsync(securityCode);
            return Result<List<OrderReservationResponseDto>>.Success(orderReservationDtos,200);
        }
        catch (Exception e)
        {
            return Result<List<OrderReservationResponseDto>>.Failure("Erro ao busacar lista de compra", 500);
        }
    }
}