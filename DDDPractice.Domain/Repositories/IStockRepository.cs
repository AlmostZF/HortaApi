
using DDD_Practice.DDDPractice.Domain.Entities;

namespace DDD_Practice.DDDPractice.Domain.Repositories;

public interface IStockRepository
{
    Task<IEnumerable<StockEntity>> GetAllAsync();
    Task<StockEntity?> GetByProductIdAsync(Guid productId);
    Task<StockEntity> GetByIdAsync(Guid stockId);
    Task UpdateQuantityAsync(StockEntity stockEntity);
    Task AddAsync(StockEntity stock);

}