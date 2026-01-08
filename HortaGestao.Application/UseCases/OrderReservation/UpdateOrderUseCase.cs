using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.OrderReservation;

public class UpdateOrderUseCase
{
    private readonly IOrderReservationService _orderReservationService;

    public UpdateOrderUseCase(IOrderReservationService orderReservationService)
    {
        _orderReservationService = orderReservationService;
    }

    public async Task<Result> ExecuteAsync(OrderReservationUpdateDto orderReservationUpdateDto)
    {
        try
        {
            await _orderReservationService.UpdateAsync(orderReservationUpdateDto);
            return Result.Success("Reserva atualizada com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao atualizar lista de compra", 500);
        }
    }
}