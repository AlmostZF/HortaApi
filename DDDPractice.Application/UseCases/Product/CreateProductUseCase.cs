using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Services;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.Product;

public class CreateProductUseCase
{
    private readonly ProductService _productService;

    public CreateProductUseCase(ProductService productService)
    {
        _productService = productService;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(ProductCreateDTO productCreateDTO)
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