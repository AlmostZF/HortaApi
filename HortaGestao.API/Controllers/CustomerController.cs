using System.Security.Claims;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Request.ProductCreateDTO;
using HortaGestao.Application.UseCases;
using HortaGestao.Application.UseCases.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HortaGestao.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CustomerController: ControllerBase
{
    private readonly CreateCustomerUseCase _createCustomerUseCase;
    private readonly UpdateCustomerUseCase _updateCustomerUseCase;
    private readonly DeleteCustomerUseCase _deleteCustomerUseCase;
    private readonly GetAllCustomerUseCase _getAllCustomerUseCase;
    private readonly GetCustomerUseCase _getUserUseCase;
    

    public CustomerController(
        CreateCustomerUseCase createCustomerUseCase,
        DeleteCustomerUseCase deleteCustomerUseCase,
        GetAllCustomerUseCase getAllCustomerUseCase,
        GetCustomerUseCase getUserUseCase,
        UpdateCustomerUseCase updateCustomerUseCase
        )
    {
        _createCustomerUseCase = createCustomerUseCase;
        _updateCustomerUseCase = updateCustomerUseCase;
        _deleteCustomerUseCase = deleteCustomerUseCase;
        _getAllCustomerUseCase = getAllCustomerUseCase;
        _getUserUseCase = getUserUseCase;
    }
    //[Authorize(Roles = "Admin")]
    [Authorize] 
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        
        var result = await _getAllCustomerUseCase.ExecuteAsync();
        
        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        
        var userClaims = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(string.IsNullOrEmpty(userClaims))
            return Unauthorized();
        
        var currentUserId = Guid.Parse(userClaims);
        var result = await _getUserUseCase.ExecuteAsync(id);

        return result.Value != null
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }
    
    [Authorize(Roles = "Customer")]
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] CustomerUpdateDto customerUpdateDto)
    {
        var result = await _updateCustomerUseCase.ExecuteAsync(customerUpdateDto);

        return result.Message != null
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CustomerCreateDto customerCreateDto)
    {
        var result = await _createCustomerUseCase.ExecuteAsync(customerCreateDto);

        return result.Value != null
            ? Created()
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {

        var result = await _deleteCustomerUseCase.ExecuteAsync(id);

        return result.Message != null
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
}