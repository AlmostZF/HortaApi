using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Request.ProductCreateDTO;
using HortaGestao.Application.Interfaces;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;


namespace HortaGestao.Application.UseCases.Stock;

public class CreateStockUseCase
{
    private readonly IStockService _stockService;

    public CreateStockUseCase(IStockService stockService)
    {
        _stockService = stockService;
    }

    public async Task<Result> ExecuteAsync(StockCreateDto stockCreate)
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