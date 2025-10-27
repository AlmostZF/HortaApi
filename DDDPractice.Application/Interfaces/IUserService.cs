using DDD_Practice.DDDPractice.Domain.Entities;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;

namespace DDDPractice.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> GetByIdAsync(Guid id);
    Task<List<UserResponseDto>> GetAllAsync(); 
    Task<Guid> CreateAsync(UserCreateDTO userCreateDto);
    Task UpdateAsync(UserUpdateDto userUpdateDTO);
    Task DeleteAsync(Guid id);
}