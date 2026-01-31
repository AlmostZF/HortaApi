using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Product;

public class UpdateProductUseCase
{
    private readonly IProductService _productService;
    private readonly IAuthRepository _authRepository;

    public UpdateProductUseCase(IProductService productService, IAuthRepository authRepository)
    {
        _productService = productService;
        _authRepository = authRepository;
    }

    public async Task<Result> ExecuteAsync(ProductUpdateDto productUpdateDTO, Guid id)
    {
        try
        {
            var sellerId = await _authRepository.GetBusinessIdByIdentityIdAsync(id);
            if (sellerId == null)
                return Result.Failure("Identificação do usuário inválida.");
            
            await _productService.UpdateAsync(productUpdateDTO, sellerId.Value);
            return Result.Success("Produto atualizado com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao atualizar produto", 500);
        }
    }
}