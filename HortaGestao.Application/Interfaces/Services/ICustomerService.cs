using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;

namespace HortaGestao.Application.Interfaces.Services;

public interface ICustomerService
{
    Task<CustomerResponseDto> GetByIdAsync(Guid id);
    Task<List<CustomerResponseDto>> GetAllAsync(); 
    Task<Guid> CreateAsync(CustomerCreateDto customerCreateDto);
    Task UpdateAsync(CustomerUpdateDto userUpdateDTO);
    Task DeleteAsync(Guid id);
}