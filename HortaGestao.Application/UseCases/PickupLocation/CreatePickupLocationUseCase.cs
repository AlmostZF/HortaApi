using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.PickupLocation;

public class CreatePickupLocationUseCase
{
    private readonly IPickupLocationService _pickupLocationService;

    public CreatePickupLocationUseCase(IPickupLocationService pickupLocationServic)
    {
        _pickupLocationService = pickupLocationServic;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(PickupLocationCreateDto pickupLocationCreateDto)
    {
        try
        {
            var pickupLocation = await _pickupLocationService.CreateAsync(pickupLocationCreateDto);
            return Result<Guid>.Success(pickupLocation, 200);
        }
        catch (Exception e)
        {
            return Result<Guid>.Failure("Erro ao criar endereço", 500);
        }
    }
}