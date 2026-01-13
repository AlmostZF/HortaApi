using HortaGestao.Domain.Entities;
using HortaGestao.Domain.IRepositories;
using HortaGestao.Infrastructure.Database.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace HortaGestao.Infrastructure.Repositories;

public class SellerRepository: ISellerRepository
{
    private readonly AppDbContext _context;

    public SellerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SellerEntity> GetByIdAsync(Guid id)
    {
        var seller = await _context.Seller.FindAsync(id);
        if (seller == null)
            throw new Exception("Seller not found.");
        
        return seller; 
    }

    public async Task AddAsync(SellerEntity seller)
    {
        _context.Seller.Add(seller);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SellerEntity seller)
    {
        _context.Seller.Update(seller);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var seller = await _context.Seller.FindAsync(id);
        if (seller == null)
            throw new Exception("Seller not found.");
        
        _context.Seller.Remove(seller);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<SellerEntity>> GetAllAsync()
    {
        return await _context.Seller
            .Include(s => s.PickupLocations)
            .ToListAsync();
    }
}