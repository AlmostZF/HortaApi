using HortaGestao.Domain.Entities;

namespace HortaGestao.Domain.IRepositories;

public interface ISellerRepository
{
    Task<SellerEntity> GetByIdAsync(Guid id);
    Task AddAsync(SellerEntity seller);
    Task UpdateAsync(SellerEntity seller);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<SellerEntity>> GetAllAsync();
}