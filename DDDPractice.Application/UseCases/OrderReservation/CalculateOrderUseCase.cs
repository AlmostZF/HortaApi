using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Services;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.OrderReservation;

public class CalculateOrderUseCase
{
    private readonly OrderReservationService _orderReservationService;

    public CalculateOrderUseCase(OrderReservationService orderReservationService)
    {
        _orderReservationService = orderReservationService;
    }

    public async Task<Result<OrderCalculateResponseDto>> ExecuteAsync(OrderCalculateDTO orderCalculateDTO)
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