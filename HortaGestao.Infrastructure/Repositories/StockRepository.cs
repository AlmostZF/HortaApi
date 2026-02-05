using HortaGestao.Domain.Entities;
using HortaGestao.Domain.IRepositories;
using HortaGestao.Infrastructure.Database.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace HortaGestao.Infrastructure.Repositories;

public class StockRepository : IStockRepository
{
    private readonly AppDbContext _context;

    public StockRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StockEntity>> GetAllAsync(Guid sellerId)
    {
        return await _context.Stock
            .Include(s=>s.Product)
            .ThenInclude(p=> p.Seller)
            .Where(s=> s.Product.SellerId == sellerId)
            .ToListAsync();
    }

    public async Task<StockEntity?> GetByProductIdAsync(Guid productId, Guid sellerId)
    {
        var stock = await _context.Stock
            .Include(s => s.Product)
            .ThenInclude(p=> p.Seller)
            .FirstOrDefaultAsync(s => s.ProductId == productId && s.Product.SellerId == sellerId);

        return stock;
    }

    public async Task<IEnumerable<StockEntity?>> GetByProductIdsAsync(IEnumerable<Guid> productId)
    {
        var listStock = await _context.Stock
            .Where(i => productId.Contains(i.Product.Id))
            .Include(s => s.Product)
                .ThenInclude(p=> p.Seller)
            .ToListAsync();
        
        return listStock;
    }

    public async Task<StockEntity> GetByIdAsync(Guid stockId,  Guid sellerId)
    {
        var stock = await _context.Stock
            .Include(s => s.Product)
                .ThenInclude(p=> p.Seller)
            .FirstOrDefaultAsync(s => s.Id == stockId && s.Product.SellerId == sellerId);
        
        if (stock == null)
            throw new Exception("Stock not found.");


        return stock;
    }

    public async Task UpdateQuantityAsync(StockEntity stockEntity)
    {
        _context.Stock.Update(stockEntity);
        await _context.SaveChangesAsync();

    }

    public async Task UpdateRangeAsync(IEnumerable<StockEntity> stockEntities)
    {
        _context.Stock.UpdateRange(stockEntities);
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(StockEntity stock)
    {
        _context.Stock.Add(stock);
        await _context.SaveChangesAsync();
    }
}