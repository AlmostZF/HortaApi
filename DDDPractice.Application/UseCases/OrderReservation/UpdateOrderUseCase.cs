using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Services;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.OrderReservation;

public class UpdateOrderUseCase
{
    private readonly OrderReservationService _orderReservationService;

    public UpdateOrderUseCase(OrderReservationService orderReservationService)
    {
        _orderReservationService = orderReservationService;
    }

    public async Task<Result> ExecuteAsync(OrderReservationUpdateDTO orderReservationUpdateDto)
    {
        try
        {
            await _orderReservationService.UpdateAsync(orderReservationUpdateDto);
            return Result.Success("Reserva atualizada com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao atualizar lista de compra", 500);
        }
    }
}