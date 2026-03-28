using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.UnitOfWork;
using HortaGestao.Application.Shared;
using HortaGestao.Application.UseCases.Product;
using HortaGestao.Application.UseCases.Stock;

namespace HortaGestao.Application.UseCases.CreateProductWithStockUseCase;

public class CreateProductWithStockUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateProductUseCase _createProductUseCase;
    private readonly CreateStockUseCase _createStockUseCase;

    public CreateProductWithStockUseCase(IUnitOfWork unitOfWork, CreateProductUseCase createProductUseCase,
        CreateStockUseCase createStockUseCase)
    {
        _unitOfWork = unitOfWork;
        _createProductUseCase = createProductUseCase;
        _createStockUseCase = createStockUseCase;
    }

    public async Task<Result<Guid>> ExecuteAsync(ProductCreateDto productDto, int quantity, Guid sellerId)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var productResult = await _createProductUseCase.ExecuteAsync(productDto, sellerId);
            
            if (!productResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return Result<Guid>.Failure(productResult.Error);
            }
            
            var stockDto = new StockCreateDto { ProductId = productResult.Value, Quantity =  quantity};
            var stockResult = await _createStockUseCase.ExecuteAsync(stockDto, sellerId);

            if (!stockResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return Result<Guid>.Failure(stockResult.Error);
            }
            
            await _unitOfWork.CommitAsync();
            return Result<Guid>.Success(productResult.Value);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            return Result<Guid>.Failure("Erro inesperado ao processar transação.");
        }
    }
}