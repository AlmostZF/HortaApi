using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Interface;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace DDDPractice.Application.Interfaces;

public interface ITokenService
{
    Task<AuthResponseDto> CreateTokenAsync(AuthUserDto user);
    Task<bool> LogoutAsync(LogoutDto token);
    Task<AuthResponseDto> RevokeTokenAsync(RefreshTokenDTO refreshToken);
    Task<Guid> ValidatedTokenAsync(string token);
}