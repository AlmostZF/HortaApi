using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.Auth;

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