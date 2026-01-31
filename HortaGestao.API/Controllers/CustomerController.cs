using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.UseCases.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HortaGestao.API.Controllers;

[Authorize]
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
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        
        var result = await _getAllCustomerUseCase.ExecuteAsync();
        
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound();
    }

    [Authorize(Roles = "CustomerRights")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        
        var result = await _getUserUseCase.ExecuteAsync(id);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound();
    }
    
    [Authorize(Roles = "CustomerRights")]
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] CustomerUpdateDto customerUpdateDto)
    {

        var result = await _updateCustomerUseCase.ExecuteAsync(customerUpdateDto);

        return result.IsSuccess
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CustomerCreateDto customerCreateDto)
    {
        var result = await _createCustomerUseCase.ExecuteAsync(customerCreateDto);

        return result.IsSuccess
            ? Created()
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Roles = "CustomerRights")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {

        var result = await _deleteCustomerUseCase.ExecuteAsync(id);

        return result.IsSuccess
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
}