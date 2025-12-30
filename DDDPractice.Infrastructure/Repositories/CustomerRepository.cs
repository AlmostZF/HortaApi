using DDDPractice.DDDPractice.Domain.Entities;
using DDDPractice.DDDPractice.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DDDPractice.DDDPractice.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerEntity> GetByIdAsync(Guid id)
    {
        var user = await _context.Customer.FindAsync(id);
        if (user == null)
            throw new Exception("User not found.");

        return user;
    }

    public async Task CreateAsync(CustomerEntity user)
    {
        _context.Customer.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CustomerEntity user)
    {
        _context.Customer.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Customer.FindAsync(id);
        if (user == null)
            throw new Exception("User not found.");

        _context.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CustomerEntity>> GetAllAsync()
    {
        return await _context.Customer.ToListAsync();
    }
}