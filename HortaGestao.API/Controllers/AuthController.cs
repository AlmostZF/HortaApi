
using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Request.ProductCreateDTO;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Shared;
using HortaGestao.Application.UseCases.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HortaGestao.API.Controllers;

[ApiController]
[Route("api/v1/")]
public class AuthController : ControllerBase
{
    
    private readonly LoginUseCase _loginUseCase;
    private readonly RegisterUserUseCase _registerUserUseCase;
    private readonly LogoutUseCase _logoutUseCase;
    private readonly RefreshUseCase _refreshUseCase;
    
    public AuthController(LoginUseCase loginUseCase,  RegisterUserUseCase registerUserUseCase,
        LogoutUseCase logoutUseCase, RefreshUseCase refreshUseCase)
    {
        _loginUseCase = loginUseCase;
        _registerUserUseCase = registerUserUseCase;
        _logoutUseCase = logoutUseCase;
        _refreshUseCase = refreshUseCase;
    }

    [HttpPost("[controller]/login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var result = await _loginUseCase.ExecuteAsync(login);
        
        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);

    }
    
    [Authorize]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutDto refreshToken)
    {
        var result = await _logoutUseCase.ExecuteAsync(refreshToken);
        return result.IsSuccess
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Refresh(RefreshTokenDTO refreshToken)
    { 
        var result = await _refreshUseCase.ExecuteAsync(refreshToken);
        
        return result.Error == null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    
    [HttpPost("[controller]/register/costumer")]
    public async Task<IActionResult> RegisterCostumer([FromBody] CustomerCreateDto dto)
    {
        var result = await _registerUserUseCase.ExecuteAsync(dto, UserType.Customer);
        
        return result.Error == null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [HttpPost("[controller]/register/seller")]
    public async Task<IActionResult> RegisterSeller([FromBody] SellerCreateDto dto)
    {
        
        var result = await _registerUserUseCase.ExecuteAsync(dto, UserType.Seller);
        
        return result.Error == null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    


}