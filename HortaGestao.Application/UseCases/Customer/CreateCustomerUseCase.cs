using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Customer;

public class CreateCustomerUseCase
{
    private readonly ICustomerService _customerService;

    public CreateCustomerUseCase(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<Result<Guid>> ExecuteAsync(CustomerCreateDto customerCreateDto)
    {
        try
        {
            var userID = await _customerService.CreateAsync(customerCreateDto);
            return Result<Guid>.Success(userID,201);
        }
        catch (Exception e)
        {
            return Result<Guid>.Failure("Erro ao criar usuário");
        }
        
    }
}