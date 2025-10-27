using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.Stock;

public class UpdateQuantityUseCase
{
    private readonly IStockService _stockService;

    public UpdateQuantityUseCase(IStockService stockService)
    {
        _stockService = stockService;
    }

    public async Task<Result> ExecuteAsync(StockUpdateDTO stockUpdateDto)
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