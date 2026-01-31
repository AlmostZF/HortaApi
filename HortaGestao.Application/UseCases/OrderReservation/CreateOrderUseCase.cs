using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
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

    public async Task<Result<List<CreateOrderReservationResponseDto>>> ExecuteAsync(OrderReservationCreateDto orderReservationCreateDto)
    {
        try
        {
           var listCreated = await _orderReservationService.AddAsync(orderReservationCreateDto);
            return Result<List<CreateOrderReservationResponseDto>>.Success(listCreated,200);
        }
        catch (Exception e)
        {
            return Result<List<CreateOrderReservationResponseDto>>.Failure("Erro ao criar lista de compra", 500);
        }
    }
}