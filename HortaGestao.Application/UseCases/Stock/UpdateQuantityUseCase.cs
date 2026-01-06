using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Stock;

public class UpdateQuantityUseCase
{
    private readonly IStockService _stockService;

    public UpdateQuantityUseCase(IStockService stockService)
    {
        _stockService = stockService;
    }

    public async Task<Result> ExecuteAsync(StockUpdateDto stockUpdateDto)
    {
        try
        {
            await _stockService.UpdateQuantityAsync(stockUpdateDto);
            return Result.Success("Stock atualizado com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao atualizar estoque", 500);
        }
        
    }
}