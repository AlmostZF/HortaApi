using DDDPractice.DDDPractice.Domain.Entities;

namespace DDDPractice.DDDPractice.Domain.Repositories;

public interface ISellerRepository
{
    Task<SellerEntity> GetByIdAsync(Guid id);
    Task AddAsync(SellerEntity seller);
    Task UpdateAsync(SellerEntity seller);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<SellerEntity>> GetAllAsync();
}