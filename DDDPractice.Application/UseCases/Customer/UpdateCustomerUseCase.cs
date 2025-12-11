using DDD_Practice.DDDPractice.Domain.Entities;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases;

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