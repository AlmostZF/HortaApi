using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Seller;

public class GetSellerUseCase
{
    private readonly ISellerService _sellerService;

    public GetSellerUseCase(ISellerService sellerService)
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