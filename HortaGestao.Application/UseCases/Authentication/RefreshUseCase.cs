using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Interfaces.UnitOfWork;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Authentication;

public class RefreshUseCase
{
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _ouw;
    
    public RefreshUseCase(ITokenService tokenService, IUnitOfWork ouw)
    {
        _tokenService = tokenService;
        _ouw = ouw;
    }

    public async Task<Result<AuthResponseDto>> ExecuteAsync(RefreshTokenDTO token)
    {
        await _ouw.BeginTransactionAsync();
        try
        {
            var userGuid = await _tokenService.ValidatedTokenAsync(token.RefreshToken);
            if (userGuid == Guid.Empty)
            {
                await _ouw.RollbackAsync();
                return Result<AuthResponseDto>.Failure("Token inválido", 401);
            }
        
        
            var authResponse = await _tokenService.RevokeTokenAsync(token);
            await _ouw.CommitAsync();
            return Result<AuthResponseDto>.Success(authResponse, 200);
        }
        catch (Exception e)
        {
            await _ouw.RollbackAsync();
            return Result<AuthResponseDto>.Failure("ERRO ao realizar refresh", 500);
        }

    }
}