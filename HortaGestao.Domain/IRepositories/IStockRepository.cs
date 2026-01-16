using HortaGestao.Domain.Entities;

namespace HortaGestao.Domain.IRepositories;

public interface IStockRepository
{
    Task<IEnumerable<StockEntity>> GetAllAsync(Guid sellerId);
    Task<StockEntity?> GetByProductIdAsync(Guid productId, Guid sellerId);
    Task<StockEntity> GetByIdAsync(Guid stockId, Guid sellerId);
    Task UpdateQuantityAsync(StockEntity stockEntity);
    Task AddAsync(StockEntity stock);

}