using HortaGestao.Application.DTOs.Authentication;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Infrastructure.Database.AppDbContext;
using HortaGestao.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace HortaGestao.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;
    public AuthRepository(UserManager<ApplicationUser> userManager,
    AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    
    public async Task<AuthUserDto?> FindByEmailAsync(string email)
    {
        var identityUser = await _userManager.FindByEmailAsync(email);

        if (identityUser == null)
            return null;

        return new AuthUserDto
        {
            Id = identityUser.Id,
            Email = identityUser.Email,
            UserName = identityUser.UserName
        };
    }

    public async Task<bool> CheckPasswordAsync(AuthUserDto user, string password)
    {
        var identityUser = await _userManager.FindByEmailAsync(user.Email);

        return await _userManager.CheckPasswordAsync(identityUser, password);
    }

    public async Task<IList<string>> GetRolesAsync(AuthUserDto user)
    {
        var identityUser = await _userManager.FindByEmailAsync(user.Email);

        return await _userManager.GetRolesAsync(identityUser);
    }

    public async Task<AuthUserDto?> CreateAsync(RegisterDto dto, Guid domainId)
    {
        
        var identity = new ApplicationUser()
        {
            Email = dto.Email,
            UserName = dto.Email,
            Name = dto.Name,
            SystemUserId = domainId
        };

        var result = await _userManager.CreateAsync(identity, dto.Password);

        if (!result.Succeeded)  
        {
            var errorMessagens = result.Errors.Select(e => e.Description).ToList();
            var message = string.Join("; ", errorMessagens);
            throw new Exception(message);
        }

        return new AuthUserDto
        {
            Id = identity.Id,
            Email = identity.Email,
            UserName = identity.UserName
        };
    }

    public async Task<bool> AddToRoleAsync(string userId, string role)
    {
        try
        {  
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.AddToRoleAsync(user, role);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }

    }

    
    public async Task<UserAspNetDto?> FindByIdAsync(string userId)
    {
        var identityUser = await _userManager.FindByIdAsync(userId);
        if(identityUser == null)
            return null;
        
        var roles = await _userManager.GetRolesAsync(identityUser);
        
        return new UserAspNetDto()
        {
            Id = identityUser.Id,
            Email = identityUser.Email,
            UserName = identityUser.UserName,
            Role = roles.ToList()
        };
    }

    public async Task<Guid?> GetBusinessIdByIdentityIdAsync(Guid identityId)
    {

        var systemUserId = _context.Users.Where(u => u.Id == identityId)
            .Select(u => u.SystemUserId)
            .FirstOrDefault();
        
        if(systemUserId == null) return null;

        var sellerId = _context.Seller
            .Where(s => s.Id == systemUserId)
            .Select(s => (Guid?)s.Id)
            .FirstOrDefault();
        
        if (sellerId != null) return sellerId;
        
        var customerId = _context.Customer
            .Where(s => s.Id == systemUserId)
            .Select(s => (Guid?)s.Id)
            .FirstOrDefault();

        return customerId;
    }
}