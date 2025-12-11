
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

    [HttpPost("register/user")]
    public async Task<IActionResult> RegisterUser([FromBody] CustomerCreateDTO dto)
    {
        
        var domainUserResult = await _createCustomerUseCase.ExecuteAsync(dto);
        if (domainUserResult == null)
            return BadRequest(domainUserResult.Error);
        
        return await Register(dto, "User", domainUserResult.Value);
    }
    
    [HttpPost("register/seller")]
    public async Task<IActionResult> RegisterSeller([FromBody] SellerCreateDTO dto)
    {
        
        var domainUserResult = await _createSellerUseCase.ExecuteAsync(dto);
        if (domainUserResult == null)
            return BadRequest(domainUserResult.Error);
        
        return await Register(dto, "Seller", domainUserResult.Value); 
    }
    
    
    private async Task<IActionResult> Register(ICreateUserDTO dto, string role, Guid domainUserId)
    {
        
        var registerDto = new RegisterDTO
        {
            Email = dto.Email,
            Password = dto.Password,
            Name = dto.Name
        };
        
        var result = await _registerUserUseCase.ExecuteAsync(registerDto, domainUserId, role);
        
        return result != null
            ? Ok(result)
            : BadRequest(result.Error);
    }
    



}