using DDD_Practice.DDDPractice.Domain.Entities;

namespace DDD_Practice.DDDPractice.Domain.Repositories;

public interface ISellerRepository
{
    Task<SellerEntity> GetByIdAsync(Guid id);
    Task AddAsync(SellerEntity seller);
    Task UpdateAsync(SellerEntity seller);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<SellerEntity>> GetAllAsync();
}