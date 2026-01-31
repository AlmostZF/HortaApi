using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.OrderReservation;

public class GetOrderUseCase
{
    private readonly IOrderReservationService _orderReservationService;
    private readonly IAuthRepository _authRepository;

    public GetOrderUseCase(IOrderReservationService orderReservationService, IAuthRepository authRepository)
    {
        _orderReservationService = orderReservationService;
        _authRepository = authRepository;
    }

    public async Task<Result<OrderReservationResponseDto>> ExecuteAsync(Guid id)
    {
        try
        {
            var customerId = await _authRepository.GetBusinessIdByIdentityIdAsync(id);
            if (customerId == null)
                return Result<OrderReservationResponseDto>.Failure("Identificação do usuário inválida.", 401);
            
            var orderReservationDto = await _orderReservationService.GetBySellerIdAsync(customerId.Value);
            
            return Result<OrderReservationResponseDto>.Success(orderReservationDto,200);
        }
        catch (Exception e)
        {
            return Result<OrderReservationResponseDto>.Failure("Erro ao criar lista de compra", 500);
        }
    }
}