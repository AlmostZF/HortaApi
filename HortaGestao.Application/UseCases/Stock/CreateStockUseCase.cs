using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;


namespace HortaGestao.Application.UseCases.Stock;

public class CreateStockUseCase
{
    private readonly IStockService _stockService;
    private readonly IAuthRepository _authRepository;

    public CreateStockUseCase(IStockService stockService,  IAuthRepository authRepository)
    {
        _stockService = stockService;
        _authRepository = authRepository;
    }

    public async Task<Result> ExecuteAsync(StockCreateDto stockCreate, Guid identityId)
    {
        try
        {
            var id = await _authRepository.GetBusinessIdByIdentityIdAsync(identityId);
            if (id == null)
                return Result.Failure("Usuário não encontrado.", 404);
            
            Console.WriteLine(id);
            await _stockService.AddAsync(stockCreate, id.Value);
            return Result.Success("Stock criado com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao criar estoque", 500);
        }
        
    }
}