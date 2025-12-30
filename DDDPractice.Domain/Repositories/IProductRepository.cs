using DDDPractice.DDDPractice.Domain.Entities;
using DDDPractice.DDDPractice.Domain.ValueObjects;

namespace DDDPractice.DDDPractice.Domain.Repositories;

public interface IProductRepository
{
    Task<ProductEntity> GetByIdAsync(Guid id);
    Task UpdateAsync (ProductEntity product);
    Task DeleteAsync(Guid id);
    Task AddAsync(ProductEntity product);
    Task<IEnumerable<ProductEntity>> GetAllAsync();
    Task<IEnumerable<ProductEntity>> FilterAsync(ProductFilter productFilter);
    Task<int> CountAsync(ProductFilter productFilter);

}