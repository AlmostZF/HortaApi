using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Product;

public class CreateProductUseCase
{
    private readonly IProductService _productService;
    private readonly IAuthRepository _authRepository;

    public CreateProductUseCase(IProductService productService,
        IAuthRepository authRepository)
    {
        _productService = productService;
        _authRepository = authRepository;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(ProductCreateDto productCreateDTO, Guid id)
    {
        try
        {
            var sellerId = await _authRepository.GetBusinessIdByIdentityIdAsync(id);
            if (sellerId == null)
                return Result<Guid>.Failure("Identificação do usuário inválida.");

            var guid = await _productService.AddAsync(productCreateDTO, sellerId.Value);
            return Result<Guid>.Success( guid,200);
        }
        catch (Exception e)
        {
            return Result<Guid>.Failure(e.Message, 500);
        }
    }
}