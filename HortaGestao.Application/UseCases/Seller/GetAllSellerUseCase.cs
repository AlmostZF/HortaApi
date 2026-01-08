using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Seller;

public class GetAllSellerUseCase
{
    private readonly ISellerService _sellerService;

    public GetAllSellerUseCase(ISellerService sellerService)
    {
        _sellerService = sellerService;
    }
    
    public async Task<Result<List<SellerResponseDto>>> ExecuteAsync()
    {
        try
        {
            var listSellerDto = await _sellerService.GetAllAsync();
            return Result<List<SellerResponseDto>>.Success(listSellerDto,200);
        }
        catch (Exception e)
        {
            return Result<List<SellerResponseDto>>.Failure("Erro ao buscar vendedores", 500);
        }
    }
}