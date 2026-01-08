using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Product;

public class UpdateProductUseCase
{
    private readonly IProductService _productService;

    public UpdateProductUseCase(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Result> ExecuteAsync(ProductUpdateDto productUpdateDTO)
    {
        try
        {
            await _productService.UpdateAsync(productUpdateDTO);
            return Result.Success("Produto atualizado com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao atualizar produto", 500);
        }
    }
}