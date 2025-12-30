using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.Auth;

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