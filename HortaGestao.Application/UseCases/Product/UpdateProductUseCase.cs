using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Request.ProductCreateDTO;
using HortaGestao.Application.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Product;

public class UpdateProductUseCase
{
    private readonly ProductService _productService;

    public UpdateProductUseCase(ProductService productService)
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