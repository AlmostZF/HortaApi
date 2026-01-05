using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Customer;

public class UpdateCustomerUseCase
{
    private readonly ICustomerService _customerService;

    public UpdateCustomerUseCase(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<Result> ExecuteAsync(CustomerUpdateDto customerUpdateDTO)
    {
        try
        {
            await _customerService.UpdateAsync(customerUpdateDTO);

            return Result.Success("Usuário atualizado com sucesso", 200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao atualizar usuário", 500);
        }
    }
}