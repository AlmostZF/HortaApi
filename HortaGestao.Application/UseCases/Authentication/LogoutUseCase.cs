using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Authentication;

public class LogoutUseCase
{

    private readonly ITokenService _tokenService;
    
    public LogoutUseCase(ITokenService tokenService)
    {
     _tokenService = tokenService;   
    }

    public async Task<Result> ExecuteAsync(LogoutDto token)
    {
        var logout = await _tokenService.LogoutAsync(token);
        if (!logout)
            return Result.Failure("Não foi possível realizar o logout");
        
        return Result.Success("Logout realizado com sucesso");
    }
}