using DDDPractice.DDDPractice.Domain.Entities;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Mappers;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases;

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