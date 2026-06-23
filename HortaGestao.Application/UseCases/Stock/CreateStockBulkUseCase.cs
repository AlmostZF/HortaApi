using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;
using HortaGestao.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;


namespace HortaGestao.Application.UseCases.Stock;

public class CreateStockBulkUseCase
{
    private readonly IStockService _stockService;
    private readonly IAuthRepository _authRepository;

    public CreateStockBulkUseCase(IStockService stockService,  IAuthRepository authRepository)
    {
        _stockService = stockService;
        _authRepository = authRepository;
    }

    public async Task<Result> ExecuteAsync(ImportMessagingDto importData, IHubContext<ImportHub> hubContext)
    {
        try
        {
            var id = await _authRepository.GetBusinessIdByIdentityIdAsync(importData.UserId);
            if (id == null)
                return Result.Failure("Usuário não encontrado.", 404);
            
            await _stockService.CreateBulkStockAsync(importData, hubContext, id.Value);
            return Result.Success("Stock criado com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao criar estoque", 500);
        }
        
    }
}