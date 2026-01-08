using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.PickupLocation;

public class DeletePickupLocationUseCase
{
    private readonly IPickupLocationService _pickupLocationService;

    public DeletePickupLocationUseCase(IPickupLocationService pickupLocationServic)
    {
        _pickupLocationService = pickupLocationServic;
    }
    
    public async Task<Result> ExecuteAsync(Guid id)
    {
        try
        {
            await _pickupLocationService.DeleteAsync(id);
            return Result.Success("endereço removido com sucesso", 200);
        }
        catch (Exception e)
        {
            return Result.Failure("endereço removido com sucesso", 500);
        }
    }
}