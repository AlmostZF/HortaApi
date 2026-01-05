using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Customer;

public class GetCustomerUseCase
{
    public readonly ICustomerService CustomerService;

    public GetCustomerUseCase(ICustomerService customerService)
    {
        CustomerService = customerService;
    }

    public async Task<Result<CustomerResponseDto>> ExecuteAsync(Guid id)
    {
        try
        {
            var user = await CustomerService.GetByIdAsync(id);
            return Result<CustomerResponseDto>.Success(user);
        }
        catch (Exception e)
        {
            return Result<CustomerResponseDto>.Failure("Erro ao buscar usuário", 500);
        }
    }
}