using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Customer;

public class GetAllCustomerUseCase
{
    public readonly ICustomerService CustomerService;

    public GetAllCustomerUseCase(ICustomerService customerService)
    {
        CustomerService = customerService;
    }

    public async Task<Result<List<CustomerResponseDto>>> ExecuteAsync()
    {
        try
        {
            var userList = await CustomerService.GetAllAsync();
                
            return Result<List<CustomerResponseDto>>.Success(userList);
        }
        catch (Exception e)
        {
            return Result<List<CustomerResponseDto>>.Failure("Erro ao buscar usuários", 500);
        }
    }
}