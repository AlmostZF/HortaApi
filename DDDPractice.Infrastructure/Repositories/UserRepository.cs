using DDD_Practice.DDDPractice.Domain.Entities;
using DDD_Practice.DDDPractice.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DDD_Practice.DDDPractice.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserEntity> GetByIdAsync(Guid id)
    {
        var user = await _context.User.FindAsync(id);
        if (user == null)
            throw new Exception("User not found.");

        return user;
    }

    public async Task CreateAsync(UserEntity user)
    {
        _context.User.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserEntity user)
    {
        _context.User.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.User.FindAsync(id);
        if (user == null)
            throw new Exception("User not found.");

        _context.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserEntity>> GetAllAsync()
    {
        return await _context.User.ToListAsync();
    }
}