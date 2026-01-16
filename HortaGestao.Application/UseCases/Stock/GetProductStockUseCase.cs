using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Stock;

public class GetProductStockUseCase
{
    private readonly IStockService _stockService;
    private readonly IAuthRepository _authRepository;

    public GetProductStockUseCase(IStockService stockService, IAuthRepository authRepository)
    {
        _stockService = stockService;
        _authRepository = authRepository;
    }
    
    public async Task<Result<StockResponseDto>> ExecuteAsync(Guid productId, Guid identityId)
    {
        try
        {
            var sellerId = await _authRepository.GetBusinessIdByIdentityIdAsync(identityId);
            if (sellerId == null)
                return Result<StockResponseDto>.Failure("Usuário não encontrado.", 404);
            
            var stockDto = await _stockService.GetByIdAsync(productId,sellerId!.Value);
            return Result<StockResponseDto>.Success(stockDto, 200);
        }
        catch (Exception e)
        {
            return Result<StockResponseDto>.Failure("Erro ao buscar estoque", 500);
        }
        
    }
}