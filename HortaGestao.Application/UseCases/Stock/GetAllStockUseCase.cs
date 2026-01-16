using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Stock;

public class GetAllStockUseCase
{
    private readonly IStockService _stockService;
    private readonly IAuthRepository _authRepository;

    public GetAllStockUseCase(IStockService stockService,  IAuthRepository authRepository)
    {
        _stockService = stockService;
        _authRepository = authRepository;
    }
    
    public async Task<Result<List<StockResponseDto>>> ExecuteAsync(Guid identityId)
    {
        try
        {
            var id = await _authRepository.GetBusinessIdByIdentityIdAsync(identityId);
            if (id == null)
                return Result<List<StockResponseDto>>.Failure("Usuário não encontrado.", 404);
            
            Console.WriteLine(id);
            var listStockDTO = await _stockService.GetAllAsync(id.Value);
            return Result<List<StockResponseDto>>.Success(listStockDTO,200);
        }
        catch (Exception e)
        {
            return Result<List<StockResponseDto>>.Failure("Erro ao buscar estoque", 500);
        }
        
    }
}