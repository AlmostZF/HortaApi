using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Services;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.OrderReservation;

public class CreateOrderUseCase
{
    private readonly OrderReservationService _orderReservationService;

    public CreateOrderUseCase(OrderReservationService orderReservationService)
    {
        _orderReservationService = orderReservationService;
    }

    public async Task<Result> ExecuteAsync(OrderReservationCreateDTO orderReservationCreateDto)
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