using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Request.ProductCreateDTO;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Mappers;
using HortaGestao.Domain.Repositories;

namespace HortaGestao.Application.Services;

public class CustomerService: ICustomerService
{
    private readonly ICustomerRepository _userRepository;
    
    public CustomerService(ICustomerRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Guid> CreateAsync(CustomerCreateDto customerCreateDto)
    {
        var user = CustomerMapper.ToCreateEntity(customerCreateDto);
        
        await _userRepository.CreateAsync(user);
        return user.Id;
    }
    
    public async Task<CustomerResponseDto> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return null;
        }
        
        return new CustomerResponseDto(user.SecurityCode)
        {
            Id = user.Id,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task<List<CustomerResponseDto>> GetAllAsync()
    {
        var listUser = await _userRepository.GetAllAsync();
        return CustomerMapper.ToDtoList(listUser);
    }
    

    public async Task UpdateAsync(CustomerUpdateDto CustomerUpdateDTO)
    {
        var existingUser = await _userRepository.GetByIdAsync(CustomerUpdateDTO.Id);
        if (existingUser == null)
        {
            throw new InvalidOperationException("Usuário não encontrado.");
        }
        
        CustomerMapper.ToUpdateEntity(existingUser, CustomerUpdateDTO);
        
        await _userRepository.UpdateAsync(existingUser);
        
    }

    public async Task DeleteAsync(Guid id)
    {
        await _userRepository.DeleteAsync(id);
    }
}