using HortaGestao.Application.DTOs.Authentication;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;

namespace HortaGestao.Application.Interfaces.Services;

public interface ITokenService
{
    Task<AuthResponseDto> CreateTokenAsync(AuthUserDto user);
    Task<bool> LogoutAsync(LogoutDto token);
    Task<AuthResponseDto> RevokeTokenAsync(RefreshTokenDTO refreshToken);
    Task<Guid> ValidatedTokenAsync(string token);
}