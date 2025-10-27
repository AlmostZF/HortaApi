using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases;

public class DeleteUserUseCase
{
    private readonly IUserService _userService;

    public DeleteUserUseCase(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> ExecuteAsync(Guid id)
    {
        try
        {
            await _userService.DeleteAsync(id);
            return Result.Success("Usuário deletado com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao deletar usuário", 500);
        }
    }
}