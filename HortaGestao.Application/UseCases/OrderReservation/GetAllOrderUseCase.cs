using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.OrderReservation;

public class GetAllOrderUseCase
{
    private readonly IOrderReservationService _orderReservationService;

    public GetAllOrderUseCase(IOrderReservationService orderReservationService)
    {
        _orderReservationService = orderReservationService;
    }

    public async Task<Result<List<OrderReservationResponseDto>>> ExecuteAsync()
    {
        try
        {
            var orderReservationDtos = await _orderReservationService.GetAllAsync();
            return Result<List<OrderReservationResponseDto>>.Success(orderReservationDtos,200);
        }
        catch (Exception e)
        {
            return Result<List<OrderReservationResponseDto>>.Failure("Erro ao buscar lista de compra", 500);
        }
    }
}