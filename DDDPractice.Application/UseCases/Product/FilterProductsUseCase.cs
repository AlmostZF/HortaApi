using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Services;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.Product;

public class FilterProductsUseCase
{
    private readonly ProductService _productService;

    public FilterProductsUseCase(ProductService productService)
    {
        _productService = productService;
    }

    public async Task<Result<PagedResponse<ProductResponseDto>>> ExecuteAsync(ProductFilterDTO productFilterDto)
    {
        try
        {
            var listProductFiltered = await _productService.FilterAsync(productFilterDto);
            return Result<PagedResponse<ProductResponseDto>>.Success(listProductFiltered, 200);
        }
        catch (Exception e)
        {
            return Result<PagedResponse<ProductResponseDto>>.Failure("Erro ao filtrar itens", 500);
        }
    }
}