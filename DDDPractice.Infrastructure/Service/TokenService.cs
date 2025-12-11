using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Interface;
using DDDPractice.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;


namespace DDDPractice.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<IdentityUser> _userManager;
    
    public TokenService(IConfiguration config)
    {
        _config = config;
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

        return await Task.FromResult(new AuthResponseDto
        {
            Token = tokenSting,
            Expiration = token.ValidTo
        });
    }
    
}