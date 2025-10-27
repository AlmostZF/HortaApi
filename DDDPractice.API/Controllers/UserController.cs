using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace DDDPractice.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController: ControllerBase
{
    private readonly CreateUserUseCase _createUserUseCase;
    private readonly UpdateUserUseCase _updateUserUseCase;
    private readonly DeleteUserUseCase _deleteUserUseCase;
    private readonly GetAllUserUseCase _getAllUserUseCase;
    private readonly GetUserUserCase _getUserUseCase;
    

    public UserController(
        CreateUserUseCase createUserUseCase,
        DeleteUserUseCase deleteUserUseCase,
        GetAllUserUseCase getAllUserUseCase,
        GetUserUserCase getUserUseCase,
        UpdateUserUseCase updateUserUseCase
        )
    {
        _createUserUseCase = createUserUseCase;
        _updateUserUseCase = updateUserUseCase;
        _deleteUserUseCase = deleteUserUseCase;
        _getAllUserUseCase = getAllUserUseCase;
        _getUserUseCase = getUserUseCase;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getAllUserUseCase.ExecuteAsync();
        
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
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto userUpdateDto)
    {
        var result = await _updateUserUseCase.ExecuteAsync(userUpdateDto);

        return result.Message != null
            ? Ok(result.Message)
            : BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO userCreateDto)
    {
        var result = await _createUserUseCase.ExecuteAsync(userCreateDto);

        return result.Value != null
            ? Created()
            : BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {

        var result = await _deleteUserUseCase.ExecuteAsync(id);

        return result.Message != null
            ? Ok(result.Message)
            : BadRequest(result.Error);
    }
}