using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Seller;

public class GetSellerUseCase
{
    private readonly SellerService _sellerService;

    public GetSellerUseCase(SellerService sellerService)
    {
        _sellerService = sellerService;
    }
    
    public async Task<Result<SellerResponseDto>> ExecuteAsync(Guid id)
    {
        try
        {
            var sellerDto = await _sellerService.GetByIdAsync(id);
            return Result<SellerResponseDto>.Success(sellerDto,200);
        }
        catch (Exception e)
        {
            return Result<SellerResponseDto>.Failure("Erro ao buscar vendedor", 500);
        }
    }
}