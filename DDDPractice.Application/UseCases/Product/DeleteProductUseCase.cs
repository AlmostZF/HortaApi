using DDDPractice.Application.Services;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.Product;

public class DeleteProductUseCase
{
    private readonly ProductService _productService;

    public DeleteProductUseCase(ProductService productService)
    {
        _productService = productService;
    }
    
    public async Task<Result> ExecuteAsync(Guid id)
    {
        try
        {
            await _productService.DeleteAsync(id);
            return Result.Success("Produto deletado com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao remover produto", 500);
        }
    }
}