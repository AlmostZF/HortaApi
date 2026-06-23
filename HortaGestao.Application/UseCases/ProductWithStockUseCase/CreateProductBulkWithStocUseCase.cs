using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Interfaces.UnitOfWork;
using HortaGestao.Application.Shared;
using HortaGestao.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;

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
    
    public async Task<Result<MessagingLogDto>> ExecuteAsync(ImportMessagingDto importData, IHubContext<ImportHub> hubContext)
    {
        try
        {
            var id = await _authRepository.GetBusinessIdByIdentityIdAsync(importData.UserId);
            if (id == null)
            {
                //TODO bulkImportResult
                return Result<MessagingLogDto>.Failure("Usuário não encontrado.", 404);
                
            }

            var productList = importData.Products;
            int total = productList.Count;
 
            int current = 0;
            
            await _unitOfWork.BeginTransactionAsync();
            
            var result = await _stockService.CreateBulkStockAsync(importData, hubContext, id.Value);
            
            Console.WriteLine($" [x] Sent {result}");
            
            await _unitOfWork.CommitAsync();
            return Result<MessagingLogDto>.Success(result, 200);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            return Result<MessagingLogDto>.Failure($"Erro ao persistir o lote no banco: {e.Message}");
        }
    }
}