using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace DDDPractice.API.Controllers;

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
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getAllCustomerUseCase.ExecuteAsync();
        
        return result.Value != null
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _getUserUseCase.ExecuteAsync(id);

        return result.Value != null
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] CustomerUpdateDto customerUpdateDto)
    {
        var result = await _updateCustomerUseCase.ExecuteAsync(customerUpdateDto);

        return result.Message != null
            ? Ok(result.Message)
            : BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CustomerCreateDTO customerCreateDto)
    {
        var result = await _createCustomerUseCase.ExecuteAsync(customerCreateDto);

        return result.Value != null
            ? Created()
            : BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {

        var result = await _deleteCustomerUseCase.ExecuteAsync(id);

        return result.Message != null
            ? Ok(result.Message)
            : BadRequest(result.Error);
    }
}