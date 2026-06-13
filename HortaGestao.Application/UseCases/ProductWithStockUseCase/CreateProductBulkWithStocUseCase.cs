using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.UnitOfWork;
using HortaGestao.Application.Shared;
using HortaGestao.Application.UseCases.Product;
using HortaGestao.Application.UseCases.Stock;

namespace HortaGestao.Application.UseCases.CreateProductWithStockUseCase;

public class CreateProductBulkWithStocUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateProductsBulkUseCase _createProductBulkUseCase;
    private readonly CreateStockBulkUseCase _createStockBulkUseCase;

    public CreateProductBulkWithStocUseCase(IUnitOfWork unitOfWork, CreateProductsBulkUseCase createProductBulkUseCase,
        CreateStockBulkUseCase createStockBulkUseCase)
    {
        _unitOfWork = unitOfWork;
        _createProductBulkUseCase = createProductBulkUseCase;
        _createStockBulkUseCase = createStockBulkUseCase;
    }
    
    public async Task<Result> ExecuteAsync(List<ProductCreateDto> productList, Guid sellerId)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _createProductBulkUseCase.ExecuteAsync(productList, sellerId);
            await _createStockBulkUseCase.ExecuteAsync(productList, sellerId);

            await _unitOfWork.CommitAsync();
            return Result.Success("Sucesso", 200);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            return Result.Failure($"Erro ao persistir o lote no banco: {e.Message}");
        }
    }
}