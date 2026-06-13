using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Product;

public class CreateProductsBulkUseCase
{
    private readonly IProductService _productService;
    private readonly IAuthRepository _authRepository;

    CreateProductsBulkUseCase(IProductService productService, IAuthRepository authRepository)
    {
        _authRepository = authRepository;
        _productService = productService;
    }

    public async Task<Result> ExecuteAsync(List<ProductCreateDto> productCreateDTO, Guid id)
    {
        try
        {
            var sellerId = await _authRepository.GetBusinessIdByIdentityIdAsync(id);
            if (sellerId == null)
                return Result.Failure("Identificação do usuário inválida.");

            await _productService.AddRangeAsync(productCreateDTO, sellerId.Value);
            return Result.Success( "sucerro",200);
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message, 500);
        }
    }
}