using DDDPractice.Application.DTOs.Interface;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;

namespace DDDPractice.DDDPractice.Domain.Repositories;

public interface IAuthRepository
{
    Task<AuthUserDto?> FindByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(AuthUserDto user, string password);
    Task<IList<string>> GetRolesAsync(AuthUserDto user);
    Task<AuthUserDto?> CreateAsync(RegisterDTO dto, Guid domainId);
    Task<bool> AddToRoleAsync(string userId, string role);
    Task<UserAspNetDto?> FindByIdAsync(string userId);
}