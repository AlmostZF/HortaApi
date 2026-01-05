using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Request.ProductCreateDTO;
using HortaGestao.Application.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Seller;

public class CreateSellerUseCase
{
    private readonly SellerService _sellerService;

    public CreateSellerUseCase(SellerService sellerService)
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
