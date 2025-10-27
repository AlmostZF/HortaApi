using DDDPractice.Application.DTOs;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.Stock;

public class GetProductStockUseCase
{
    private readonly IStockService _stockService;

    public GetProductStockUseCase(IStockService stockService)
    {
        _stockService = stockService;
    }
    
    public async Task<Result<StockResponseDto>> ExecuteAsync(Guid productId)
    {
        try
        {
            var stockDto = await _stockService.GetByIdAsync(productId);
            return Result<StockResponseDto>.Success(stockDto, 200);
        }
        catch (Exception e)
        {
            return Result<StockResponseDto>.Failure("Erro ao buscar estoque", 500);
        }
        
    }
}