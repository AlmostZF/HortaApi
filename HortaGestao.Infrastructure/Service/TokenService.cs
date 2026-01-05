using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request.ProductCreateDTO;
using HortaGestao.Application.Interfaces;
using HortaGestao.Application.DTOs.Authentication;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IRefreshTokenRespository _refreshTokenRespository;
    private readonly IAuthRepository _authRepository;
    
    public TokenService(IConfiguration config, IRefreshTokenRespository refreshTokenRespository,
        IAuthRepository authRepository)
    {
        _config = config;
        _refreshTokenRespository = refreshTokenRespository;
        _authRepository = authRepository;
    }
    
    public async Task<AuthResponseDto> CreateTokenAsync(AuthUserDto user)
    {
        var jwtSection = _config.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSection["Key"] ?? string.Empty);
        var issuer = jwtSection["Issuer"];
        var audience = jwtSection["Audience"];
        var durationMinutes = int.Parse(jwtSection["DurationMinutes"] ?? string.Empty);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in user.Roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256
            );

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(durationMinutes),
            signingCredentials: creds
            );

        var tokenSting = new JwtSecurityTokenHandler().WriteToken(token);

        var refreshToken = await GenerateRefreshToken(user);

        return await Task.FromResult(new AuthResponseDto
        {
            BearerToken = tokenSting,
            Expiration = token.ValidTo,
            RefreshToken = refreshToken,
        });
    }
    

    public async Task<bool> LogoutAsync(LogoutDto token)
    {
        var hexToken = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token.RefreshToken)));
        var logout = await _refreshTokenRespository.DeleteAsync(hexToken);
        return logout;
    }

    public async Task<AuthResponseDto> RevokeTokenAsync(RefreshTokenDTO refreshToken)
    {
        var hexToken = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken.RefreshToken)));
        
        var userId = await ValidatedTokenAsync(refreshToken.RefreshToken);
        if(userId == Guid.Empty)
            return null;
        
        var aspNetUser = await _authRepository.FindByIdAsync(userId.ToString());

        var userDto = new AuthUserDto()
        {
            Id = userId,
            Email = aspNetUser.Email,
            UserName = aspNetUser.UserName,
            Roles = aspNetUser.Role,
            Token = hexToken
        };
        return await CreateTokenAsync(userDto);
    }

    public async Task<Guid> ValidatedTokenAsync(string refreshToken)
    {
        var hexToken = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));
        var refreshmentEntity = await _refreshTokenRespository.GetByTokenAsync(hexToken);
        
        if (refreshmentEntity is not { IsActive: true })
            return Guid.Empty;
        
        return refreshmentEntity.UserId;
    }
    
    private async Task<string> GenerateRefreshToken(AuthUserDto user)
    {
        
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshTokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));

        if (!string.IsNullOrEmpty(user.Token))
        {
            var oldTokenEntity = await _refreshTokenRespository.GetByTokenAsync(user.Token);

            if (oldTokenEntity != null)
            {
                oldTokenEntity.Revoke("Replaced by new token", refreshTokenHash);
                
                await _refreshTokenRespository.UpdateAsync(oldTokenEntity);
                
            }
        }
        
        var newToken = new RefreshTokenEntity()
        {
            UserId = user.Id,
            AbsoluteExpires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(1),
            Token = refreshTokenHash,
            IsRevoked = false,
            ReplacedByToken = null,
            Id = Guid.NewGuid()
        };
        await _refreshTokenRespository.CreateAsync(newToken);
        return refreshToken;
    }
    
    

}