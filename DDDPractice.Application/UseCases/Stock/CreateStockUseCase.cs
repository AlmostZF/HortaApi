using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Shared;


namespace DDDPractice.Application.UseCases.Stock;

public class CreateStockUseCase
{
    private readonly IStockService _stockService;

    public CreateStockUseCase(IStockService stockService)
    {
        _stockService = stockService;
    }

    public async Task<Result> ExecuteAsync(StockCreateDTO stockCreate)
    {
        try
        {
            await _stockService.AddAsync(stockCreate);
            return Result.Success("Stock criado com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao criar estoque", 500);
        }
        
    }
}