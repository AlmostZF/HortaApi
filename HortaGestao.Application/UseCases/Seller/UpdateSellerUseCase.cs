using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Seller;

public class UpdateSellerUseCase
{
    private readonly ISellerService _sellerService;
    private readonly IAuthRepository _authRepository;

    public UpdateSellerUseCase(ISellerService sellerService,  IAuthRepository authRepository)
    {
        _sellerService = sellerService;
        _authRepository = authRepository;
    }

    public async Task<Result> ExecuteAsync(SellerUpdateDto sellerUpdateDTO, Guid id)
    {
        try
        {
            var sellerId = await _authRepository.GetBusinessIdByIdentityIdAsync(id);
            if (sellerId == null)
                return Result.Failure("Identificação do usuário inválida.");
            
            await _sellerService.UpdateAsync(sellerUpdateDTO);
            return Result.Success("Vendedor atualizado com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao atualizar vendedor", 500);
        }
    }
}