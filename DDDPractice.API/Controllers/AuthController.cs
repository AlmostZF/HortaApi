
using DDD_Practice.DDDPractice.Domain.Repositories;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.UseCases;
using DDDPractice.Application.UseCases.Auth;
using DDDPractice.Application.UseCases.Seller;
using Microsoft.AspNetCore.Mvc;

namespace DDDPractice.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    
    private readonly LoginUseCase _loginUseCase;
    private readonly RegisterUserUseCase _registerUserUseCase;
    private readonly CreateCustomerUseCase _createCustomerUseCase;
    private readonly CreateSellerUseCase _createSellerUseCase;
    
    public AuthController(LoginUseCase loginUseCase,  RegisterUserUseCase registerUserUseCase,
        CreateCustomerUseCase createCustomerUseCase, CreateSellerUseCase createSellerUseCase)
    {
        _loginUseCase = loginUseCase;
        _createCustomerUseCase = createCustomerUseCase;
        _registerUserUseCase = registerUserUseCase;
        _createSellerUseCase = createSellerUseCase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto login)
    {
        var result = await _loginUseCase.ExecuteAsync(login);
        
        return result.Value != null
            ? Ok(result.Value)
            : BadRequest(result.Error);

    }

    [HttpPost("register/costumer")]
    public async Task<IActionResult> RegisterCostumer([FromBody] CustomerCreateDTO dto)
    {
        var result = await _registerUserUseCase.ExecuteAsync(dto);
        
        return result.Value != null
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }
    
    [HttpPost("register/seller")]
    public async Task<IActionResult> RegisterSeller([FromBody] SellerCreateDTO dto)
    {
        
        var result = await _registerUserUseCase.ExecuteAsync(dto);
        
        return result.Value != null
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }
    


}