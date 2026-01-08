using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Product;

public class CreateProductUseCase
{
    private readonly IProductService _productService;

    public CreateProductUseCase(IProductService productService)
    {
        _productService = productService;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(ProductCreateDto productCreateDTO)
    {
        try
        {
            var guid = await _productService.AddAsync(productCreateDTO);
            return Result<Guid>.Success( guid,200);
        }
        catch (Exception e)
        {
            return Result<Guid>.Failure(e.Message, 500);
        }
    }
}