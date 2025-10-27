using DDD_Practice.DDDPractice.Domain.Entities;
using DDD_Practice.DDDPractice.Domain.Repositories;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DDD_Practice.DDDPractice.Infrastructure.Repositories;

public class StockRepository : IStockRepository
{
    private readonly AppDbContext _context;

    public StockRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StockEntity>> GetAllAsync()
    {
        return await _context.Stock
            .Include(s=>s.Product)
            .ThenInclude(p=> p.Seller)
            .ToListAsync();
    }

    public async Task<StockEntity?> GetByProductIdAsync(Guid productId)
    {
        var stock = await _context.Stock
            .Include(s => s.Product)
            .ThenInclude(p=> p.Seller)
            .FirstOrDefaultAsync(s => s.ProductId == productId);

        return stock;
    }

    public async Task<StockEntity> GetByIdAsync(Guid stockId)
    {
        var stock = await _context.Stock
            .Include(s => s.Product)
            .ThenInclude(p=> p.Seller)
            .FirstOrDefaultAsync(s => s.Id == stockId);
        
        if (stock == null)
            throw new Exception("Stock not found.");


        return stock;
    }

    public async Task UpdateQuantityAsync(StockEntity stockEntity)
    {
        _context.Stock.Update(stockEntity);
        await _context.SaveChangesAsync();

    }

    public async Task AddAsync(StockEntity stock)
    {
        _context.Stock.Add(stock);
        await _context.SaveChangesAsync();
    }
}