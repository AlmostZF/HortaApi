using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Stock;

public class GetStockByProductIdUseCase
{
    private readonly IStockService _stockService;

    public GetStockByProductIdUseCase(IStockService stockService)
    {
        _stockService = stockService;
    }
    
    public async Task<Result<StockAvailableResponseDto>> ExecuteAsync(Guid productId)
    {
        try
        {
            var stockAvailableResponseDto = await _stockService.GetByProductIdAsync(productId);
            return Result<StockAvailableResponseDto>.Success(stockAvailableResponseDto, 200);
        }
        catch (Exception e)
        {
            return Result<StockAvailableResponseDto>.Failure("Erro ao buscar estoque", 500);
        }
        
    }
}