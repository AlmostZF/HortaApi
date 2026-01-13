using HortaGestao.Domain.Entities;
using HortaGestao.Domain.IRepositories;
using HortaGestao.Infrastructure.Database.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace HortaGestao.Infrastructure.Repositories;

public class PickupLocationRespository : IPickupLocationRespository
{
    private readonly AppDbContext _context;

    public PickupLocationRespository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateAsync(PickupLocationEntity entity)
    {
        _context.PickupLocation.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var pickupLocation = await _context.PickupLocation.FindAsync(id);
        if (pickupLocation == null)
            throw new Exception("PickupLocation not found.");
        
        _context.PickupLocation.Remove(pickupLocation);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PickupLocationEntity entity)
    {
        var pickupLocation = await _context.PickupLocation.FindAsync(entity.Id);
        if (pickupLocation == null)
            throw new Exception("PickupLocation not found.");
        
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<PickupLocationEntity>> GetBySellerIdAsync(Guid sellerId)
    {

        return await _context.PickupLocation
            .Where(p => EF.Property<Guid>(p, "SellerId") == sellerId)
            .ToListAsync();
    }
    public async Task<PickupLocationEntity> GetByIdAsync(Guid sellerId)
    {
        var pickupLocation = await _context.PickupLocation.FindAsync(sellerId);
        if(pickupLocation == null)
            throw new Exception("PickupLocation not found.");
        
        return pickupLocation;
    }
    
}