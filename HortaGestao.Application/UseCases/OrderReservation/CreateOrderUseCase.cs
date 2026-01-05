using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Request.ProductCreateDTO;
using HortaGestao.Application.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.OrderReservation;

public class CreateOrderUseCase
{
    private readonly OrderReservationService _orderReservationService;

    public CreateOrderUseCase(OrderReservationService orderReservationService)
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