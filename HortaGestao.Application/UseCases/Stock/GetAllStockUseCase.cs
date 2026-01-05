using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Stock;

public class GetAllStockUseCase
{
    private readonly IStockService _stockService;

    public GetAllStockUseCase(IStockService stockService)
    {
        _stockService = stockService;
    }
    
    public async Task<Result<List<StockResponseDto>>> ExecuteAsync()
    {
        try
        {
            var listStockDTO = await _stockService.GetAllAsync();
            return Result<List<StockResponseDto>>.Success(listStockDTO,200);
        }
        catch (Exception e)
        {
            return Result<List<StockResponseDto>>.Failure("Erro ao buscar estoque", 500);
        }
        
    }
}