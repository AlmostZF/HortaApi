using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases;

public class CreateUserUseCase
{
    private readonly IUserService _userService;

    public CreateUserUseCase(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<Guid>> ExecuteAsync(UserCreateDTO userCreateDTO)
    {
        try
        {
            var userID = await _userService.CreateAsync(userCreateDTO);
            return Result<Guid>.Success(userID,201);
        }
        catch (Exception e)
        {
            return Result<Guid>.Failure("Erro ao criar usuário");
        }
        
    }
}