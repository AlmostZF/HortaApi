using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Customer;

public class DeleteCustomerUseCase
{
    private readonly ICustomerService _customerService;

    public DeleteCustomerUseCase(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<Result> ExecuteAsync(Guid id)
    {
        try
        {
            await _customerService.DeleteAsync(id);
            return Result.Success("Usuário deletado com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao deletar usuário", 500);
        }
    }
}