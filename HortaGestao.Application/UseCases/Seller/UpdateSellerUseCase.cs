using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Seller;

public class UpdateSellerUseCase
{
    private readonly SellerService _sellerService;

    public UpdateSellerUseCase(SellerService sellerService)
    {
        _sellerService = sellerService;
    }

    public async Task<Result> ExecuteAsync(SellerUpdateDto sellerUpdateDTO)
    {
        try
        {
            await _sellerService.UpdateAsync(sellerUpdateDTO);
            return Result.Success("Vendedor atualizado com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao atualizar vendedor", 500);
        }
    }
}