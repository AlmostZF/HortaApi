using DDDPractice.Application.DTOs;
using DDDPractice.Application.Services;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.Product;

public class GetAllProductUseCase
{
    private readonly ProductService _productService;

    public GetAllProductUseCase(ProductService productService)
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