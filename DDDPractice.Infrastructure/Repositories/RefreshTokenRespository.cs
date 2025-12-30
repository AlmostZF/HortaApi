using System.Security.Cryptography;
using System.Text;
using DDDPractice.DDDPractice.Domain.Repositories;
using DDDPractice.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDDPractice.DDDPractice.Infrastructure.Repositories;

public class RefreshTokenRespository: IRefreshTokenRespository
{
    private readonly AppDbContext _context;

    public RefreshTokenRespository(AppDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task CreateAsync(RefreshTokenEntity refreshToken)
    {
        _context.RefreshToken.Add(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshTokenEntity?> GetByTokenAsync(string token)
    {
        var refreshTokenEntity = await _context.RefreshToken.FirstOrDefaultAsync(r=> r.Token == token);
        return refreshTokenEntity;
    }

    public async Task UpdateAsync(RefreshTokenEntity refreshToken)
    {
        
        _context.RefreshToken.Update(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(string refreshToken)
    {
        
        var token = await GetByTokenAsync(refreshToken);
        if (token == null)
            return false;
        
        _context.RefreshToken.Remove(token);
        await _context.SaveChangesAsync();
        return true;
    }
}