using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.OrderReservation;

public class GetOrderBySecurityCodeUseCase
{
    private readonly OrderReservationService _orderReservationService;

    public GetOrderBySecurityCodeUseCase(OrderReservationService orderReservationService)
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