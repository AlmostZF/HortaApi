using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.OrderReservation;

public class CreateOrderUseCase
{
    private readonly IOrderReservationService _orderReservationService;

    public CreateOrderUseCase(IOrderReservationService orderReservationService)
    {
        _orderReservationService = orderReservationService;
    }

    public async Task<Result> ExecuteAsync(OrderReservationCreateDto orderReservationCreateDto)
    {
        try
        {
            await _orderReservationService.AddAsync(orderReservationCreateDto);
            return Result.Success("Reserva criada com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao criar lista de compra", 500);
        }
    }
}