using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.PickupLocation;

public class GetByIdPickupLocationUseCase
{
    private readonly IPickupLocationService _pickupLocationService;
    private  readonly IAuthRepository _authRepository;

    public GetByIdPickupLocationUseCase(IPickupLocationService pickupLocationService,  IAuthRepository authRepository)
    {
        _pickupLocationService = pickupLocationService;
        _authRepository = authRepository;
    }
    
    public async Task<Result<List<PickupLocationResponseDto>>> ExecuteAsync(Guid id)
    {
        try
        {
            
            var sellerId = await _authRepository.GetBusinessIdByIdentityIdAsync(id);
            if (sellerId == null)
                return Result<List<PickupLocationResponseDto>>.Failure("Identificação do usuário inválida.", 401);
            
            var pickupLocation = await _pickupLocationService.GetBySellerIdAsync(sellerId.Value);
            return Result<List<PickupLocationResponseDto>>.Success(pickupLocation, 200);
        }
        catch (Exception e)
        {
            return Result<List<PickupLocationResponseDto>>.Failure("Erro ao buscar endereço", 500);
        }
    }
}