using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.PickupLocation;

public class UpdatePickupLocationUseCase
{
    private readonly IPickupLocationService _pickupLocationService;

    public UpdatePickupLocationUseCase(IPickupLocationService pickupLocationServic)
    {
        _pickupLocationService = pickupLocationServic;
    }
    
    public async Task<Result> ExecuteAsync(PickupLocationUpdateDto pickupLocationUpdateDto)
    {
        try
        {
            await _pickupLocationService.UpdateAsync(pickupLocationUpdateDto);
            return Result.Success("Criado com sucesso", 200);
        }
        catch (Exception e)
        {
            return Result.Failure("Falha ao criar endereço", 500);
        }
    }
}