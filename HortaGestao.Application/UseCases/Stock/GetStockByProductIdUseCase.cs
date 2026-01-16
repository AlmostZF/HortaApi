using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Stock;

public class GetStockByProductIdUseCase
{
    private readonly IStockService _stockService;
    private readonly IAuthRepository _authRepository;


    public GetStockByProductIdUseCase(IStockService stockService,IAuthRepository authRepository)
    {
        _stockService = stockService;
        _authRepository = authRepository;
    }
    
    public async Task<Result<StockAvailableResponseDto>> ExecuteAsync(Guid productId, Guid identityId)
    {
        try
        {
            var sellerId = await _authRepository.GetBusinessIdByIdentityIdAsync(identityId);
            if (sellerId == null)
                return Result<StockAvailableResponseDto>.Failure("Usuário não encontrado.", 404);
            
            var stockAvailableResponseDto = await _stockService.GetByProductIdAsync(productId, sellerId!.Value);
            return Result<StockAvailableResponseDto>.Success(stockAvailableResponseDto, 200);
        }
        catch (Exception e)
        {
            return Result<StockAvailableResponseDto>.Failure("Erro ao buscar estoque", 500);
        }
        
    }
}