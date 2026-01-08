using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.OrderReservation;

public class GetOrderBySecurityCodeUseCase
{
    private readonly IOrderReservationService _orderReservationService;

    public GetOrderBySecurityCodeUseCase(IOrderReservationService orderReservationService)
    {
        _orderReservationService = orderReservationService;
    }

    public async Task<Result<List<OrderReservationResponseDto>>> ExecuteAsync(string securityCode)
    {
        try
        {
            var orderReservationDtos = await _orderReservationService.GetBySecurityCodeAsync(securityCode);
            return Result<List<OrderReservationResponseDto>>.Success(orderReservationDtos,200);
        }
        catch (Exception e)
        {
            return Result<List<OrderReservationResponseDto>>.Failure(e.Message, 500);
        }
    }
}