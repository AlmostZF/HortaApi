using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.OrderReservation;

public class CalculateOrderUseCase
{
    private readonly OrderReservationService _orderReservationService;

    public CalculateOrderUseCase(OrderReservationService orderReservationService)
    {
        _orderReservationService = orderReservationService;
    }

    public async Task<Result<OrderCalculateResponseDto>> ExecuteAsync(OrderCalculateDto orderCalculateDTO)
    {
        try
        {
            var OrderCalculated = await _orderReservationService.CalculateAsync(orderCalculateDTO);
            return Result<OrderCalculateResponseDto>.Success(OrderCalculated,200);
        }
        catch (Exception e)
        {
            return Result<OrderCalculateResponseDto>.Failure("Erro ao Calcular lista de compra", 500);
        }
    }
}