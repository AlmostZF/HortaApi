using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Product;

public class GetAllProductUseCase
{
    private readonly IProductService _productService;

    public GetAllProductUseCase(IProductService productService)
    {
        _productService = productService;
    }
    public async Task<Result<List<ProductResponseDto>>> ExecuteAsync()
    {
        try
        {
            var productDto = await _productService.GetAllAsync();
            return Result<List<ProductResponseDto>>.Success(productDto,200);
        }
        catch (Exception e)
        {
            return Result<List<ProductResponseDto>>.Failure("Erro ao buscar todos produtos", 500);
        }
    }
}