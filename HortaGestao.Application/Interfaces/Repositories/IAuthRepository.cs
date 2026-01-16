using HortaGestao.Application.DTOs.Authentication;
using HortaGestao.Application.DTOs.Request;

namespace HortaGestao.Application.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<AuthUserDto?> FindByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(AuthUserDto user, string password);
    Task<IList<string>> GetRolesAsync(AuthUserDto user);
    Task<AuthUserDto?> CreateAsync(RegisterDto dto, Guid domainId);
    Task<bool> AddToRoleAsync(string userId, string role);
    Task<UserAspNetDto?> FindByIdAsync(string userId);
    Task<Guid?> GetBusinessIdByIdentityIdAsync(Guid identityId);
}