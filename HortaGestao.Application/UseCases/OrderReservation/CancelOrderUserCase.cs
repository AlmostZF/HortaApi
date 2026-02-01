using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.OrderReservation;

public class CancelOrderUserCase
{
    private readonly IOrderReservationService _orderReservationService;

    public CancelOrderUserCase(IOrderReservationService orderReservationService)
    {
        _orderReservationService = orderReservationService;
    }

    public async Task<Result> ExecuteAsync(string securityCode, Guid sellerId)
    {
        try
        {
            await _orderReservationService.CancelOrderAsync(securityCode,sellerId);
            return Result.Success("Reserva atualizada com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao atualizar lista de compra", 500);
        }
    }
}