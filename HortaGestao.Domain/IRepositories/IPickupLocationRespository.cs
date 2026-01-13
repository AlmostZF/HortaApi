using HortaGestao.Domain.Entities;

namespace HortaGestao.Domain.IRepositories;

public interface IPickupLocationRespository
{
    public Task CreateAsync(PickupLocationEntity entity);
    public Task DeleteAsync(Guid id);
    public Task UpdateAsync(PickupLocationEntity entity);
    public Task<IEnumerable<PickupLocationEntity>> GetBySellerIdAsync(Guid sellerId);
    public Task<PickupLocationEntity> GetByIdAsync(Guid Id);
}