using DDDPractice.Application.DTOs;
using DDDPractice.Application.Services;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.OrderReservation;

public class DeleteOrderUseCase
{
    private readonly OrderReservationService _orderReservationService;

    public DeleteOrderUseCase(OrderReservationService orderReservationService)
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