using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Product;

public class FilterProductsUseCase
{
    private readonly IProductService _productService;

    public FilterProductsUseCase(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Result<PagedResponse<ProductResponseDto>>> ExecuteAsync(ProductFilterDto productFilterDto)
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