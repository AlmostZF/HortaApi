using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Interfaces.UnitOfWork;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.CreateProductWithStockUseCase;

public class CreateProductBulkWithStocUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStockService _stockService;
    private readonly IAuthRepository _authRepository;

    public CreateProductBulkWithStocUseCase(IUnitOfWork unitOfWork, IAuthRepository authRepository,
        IStockService stockService)
    {
        _unitOfWork = unitOfWork;
        _authRepository = authRepository;
        _stockService = stockService;
    }
    
    public async Task<Result> ExecuteAsync(List<ProductCreateDto> productList, Guid sellerId)
    {
        try
        {
            var id = await _authRepository.GetBusinessIdByIdentityIdAsync(sellerId);
            if (id == null)
            {
                //TODO bulkImportResult
                return Result.Failure("Usuário não encontrado.", 404);
                
            }

            int total = productList.Count;
            int current = 0;
            
            await _unitOfWork.BeginTransactionAsync();
            
            _stockService.CreateBulkStockAsync(productList, id.Value);
            
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