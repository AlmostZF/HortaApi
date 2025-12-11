using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases;

public class CreateCustomerUseCase
{
    private readonly ICustomerService _customerService;

    public CreateCustomerUseCase(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<Result<Guid>> ExecuteAsync(CustomerCreateDTO customerCreateDto)
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