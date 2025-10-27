using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Services;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.Seller;

public class UpdateSellerUseCase
{
    private readonly SellerService _sellerService;

    public UpdateSellerUseCase(SellerService sellerService)
    {
        _sellerService = sellerService;
    }

    public async Task<Result> ExecuteAsync(SellerUpdateDTO sellerUpdateDTO)
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