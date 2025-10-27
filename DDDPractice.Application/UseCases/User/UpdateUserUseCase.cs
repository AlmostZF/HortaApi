using DDD_Practice.DDDPractice.Domain.Entities;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases;

public class UpdateUserUseCase
{
    private readonly IUserService _userService;

    public UpdateUserUseCase(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> ExecuteAsync(UserUpdateDto userUpdateDTO)
    {
        try
        {
            await _userService.UpdateAsync(userUpdateDTO);

            return Result.Success("Usuário atualizado com sucesso", 200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao atualizar usuário", 500);
        }
    }
}