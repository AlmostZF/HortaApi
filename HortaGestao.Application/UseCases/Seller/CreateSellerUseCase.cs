using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Seller;

public class CreateSellerUseCase
{
    private readonly ISellerService _sellerService;

    public CreateSellerUseCase(ISellerService sellerService)
    {
        _sellerService = sellerService;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(SellerCreateDto sellerCreateDto)
    {
        try
        {
           var guid = await _sellerService.AddAsync(sellerCreateDto);
           return Result<Guid>.Success(guid,201);
        }
        catch (Exception e)
        {
            return Result<Guid>.Failure("Erro ao crair vendedor", 500);
        }
    }
}
