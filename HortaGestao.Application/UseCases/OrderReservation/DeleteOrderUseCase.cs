using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.OrderReservation;

public class DeleteOrderUseCase
{
    private readonly IOrderReservationService _orderReservationService;

    public DeleteOrderUseCase(IOrderReservationService orderReservationService)
    {
        _orderReservationService = orderReservationService;
    }

    public async Task<Result> ExecuteAsync(Guid id)
    {
        try
        {
            await _orderReservationService.DeleteAsync(id);
            return Result.Success("Reserva deletada com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao deletar lista de compra", 500);
        }
    }
}