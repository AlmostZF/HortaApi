using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Stock;

public class UpdateQuantityUseCase
{
    private readonly IStockService _stockService;
    private readonly IAuthRepository _authRepository;

    public UpdateQuantityUseCase(IStockService stockService,  IAuthRepository authRepository)
    {
        _stockService = stockService;
        _authRepository = authRepository;
    }

    public async Task<Result> ExecuteAsync(StockUpdateDto stockUpdateDto, Guid identityId)
    {
        try
        {
            var sellerId = await _authRepository.GetBusinessIdByIdentityIdAsync(identityId);
            if (sellerId == null)
                return Result.Failure("Usuário não encontrado.", 404);
            
            await _stockService.UpdateQuantityAsync(stockUpdateDto, identityId);
            return Result.Success("Stock atualizado com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao atualizar estoque", 500);
        }
        
    }
}