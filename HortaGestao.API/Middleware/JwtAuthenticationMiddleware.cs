using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace HortaGestao.API.Middleware;

public class JwtAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _key;
    
    public JwtAuthenticationMiddleware(RequestDelegate next, string key)
    {
        _next = next;
        _key = key;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        { 
            TokenValidate(context, token);
        }

        await _next(context);
    }

    private void TokenValidate(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);
            
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
                
            },out SecurityToken validatedToken);
            
            var jwtToken = (JwtSecurityToken)validatedToken;
            
            context.User = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(
                    jwtToken.Claims, "jwt"
                    )
                );
        }
        catch
        {

        }
    }
    
}