using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Seller;

public class DeleteSellerUseCase
{
    private readonly ISellerService _sellerService;

    public DeleteSellerUseCase(ISellerService sellerService)
    {
        _sellerService = sellerService;
    }
    
    public async Task<Result> ExecuteAsync(Guid id)
    {
        try
        {
            await _sellerService.DeleteAsync(id);
            return Result.Success("Vendedor deletado com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao remover vendedor", 500);
        }
    }
}