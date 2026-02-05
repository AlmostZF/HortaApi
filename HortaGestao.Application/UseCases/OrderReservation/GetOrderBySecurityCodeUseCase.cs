using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.OrderReservation;

public class GetOrderBySecurityCodeUseCase
{
    private readonly IOrderReservationService _orderReservationService;
    private  readonly IAuthRepository _authRepository;
    public GetOrderBySecurityCodeUseCase(IOrderReservationService orderReservationService,
        IAuthRepository authRepository)
    {
        _orderReservationService = orderReservationService;
        _authRepository = authRepository;
    }

    public async Task<Result<OrderReservationResponseDto?>> ExecuteAsync(string securityCode, Guid id)
    {
        try
        {
            var sellerId = await _authRepository.GetBusinessIdByIdentityIdAsync(id);
            if (sellerId == null)
                return Result<OrderReservationResponseDto?>.Failure("Identificação do usuário inválida.");
                    
            var orderReservationDtos = await _orderReservationService.GetBySecurityCodeAsync(securityCode.ToUpper(), sellerId!.Value);
            return orderReservationDtos != null 
                ?  Result<OrderReservationResponseDto?>.Success(orderReservationDtos,200)
                :  Result<OrderReservationResponseDto?>.Success(new OrderReservationResponseDto(),200);
        }
        catch (Exception e)
        {
            return Result<OrderReservationResponseDto?>.Failure(e.Message, 500);
        }
    }
}