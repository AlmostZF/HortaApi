using DDDPractice.DDDPractice.Domain.Entities;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;

namespace DDDPractice.Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerResponseDto> GetByIdAsync(Guid id);
    Task<List<CustomerResponseDto>> GetAllAsync(); 
    Task<Guid> CreateAsync(CustomerCreateDTO customerCreateDto);
    Task UpdateAsync(CustomerUpdateDto userUpdateDTO);
    Task DeleteAsync(Guid id);
}