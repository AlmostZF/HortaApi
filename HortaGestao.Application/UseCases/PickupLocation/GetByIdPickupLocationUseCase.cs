using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.PickupLocation;

public class GetByIdPickupLocationUseCase
{
    private readonly IPickupLocationService _pickupLocationService;

    public GetByIdPickupLocationUseCase(IPickupLocationService pickupLocationServic)
    {
        _pickupLocationService = pickupLocationServic;
    }
    
    public async Task<Result<List<PickupLocationResponseDto>>> ExecuteAsync(Guid id)
    {
        try
        {
            var pickupLocation = await _pickupLocationService.GetBySellerIdAsync(id);
            return Result<List<PickupLocationResponseDto>>.Success(pickupLocation, 200);
        }
        catch (Exception e)
        {
            return Result<List<PickupLocationResponseDto>>.Failure("Erro ao buscar endereço", 500);
        }
    }
}