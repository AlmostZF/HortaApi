using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.PickupLocation;

public class UpdatePickupLocationUseCase
{
    private readonly IPickupLocationService _pickupLocationService;
    private readonly IAuthRepository _authRepository;

    public UpdatePickupLocationUseCase(IPickupLocationService pickupLocationService,  IAuthRepository authRepositore)
    {
        _pickupLocationService = pickupLocationService;
        _authRepository = authRepositore;
    }
    
    public async Task<Result> ExecuteAsync(List<PickupLocationUpdateDto> pickupLocationUpdateDto, Guid id)
    {
        try
        {
            var sellerId = await _authRepository.GetBusinessIdByIdentityIdAsync(id);
            if (sellerId == null)
                return Result.Failure("Identificação do usuário inválida.");
            
            await _pickupLocationService.UpdateAsync(pickupLocationUpdateDto, sellerId.Value);
            return Result.Success("Criado com sucesso", 200);
        }
        catch (Exception e)
        {
            return Result.Failure("Falha ao criar endereço", 500);
        }
    }
}