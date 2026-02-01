using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.OrderReservation;

public class FinishOrderUserCase
{
    private readonly IOrderReservationService _orderReservationService;
    private readonly IAuthRepository _authRepository;

    public FinishOrderUserCase(IOrderReservationService orderReservationService,
        IAuthRepository authRepository)
    {
        _orderReservationService = orderReservationService;
        _authRepository = authRepository;
    }

    public async Task<Result> ExecuteAsync(Guid guid, Guid id)
    {
        try
        {
            var sellerId = await _authRepository.GetBusinessIdByIdentityIdAsync(id);
            if (sellerId == null)
                return Result.Failure("Identificação do usuário inválida.");

            
            await _orderReservationService.FinishOrderAsync(guid,sellerId!.Value);
            return Result.Success("Reserva atualizada com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao atualizar lista de compra", 500);
        }
    }
}