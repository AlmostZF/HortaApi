using HortaGestao.Domain.Entities;

namespace HortaGestao.Domain.Repositories;

public interface ICustomerRepository
{
    Task<CustomerEntity> GetByIdAsync(Guid id);
    Task CreateAsync(CustomerEntity user);
    Task UpdateAsync(CustomerEntity user);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<CustomerEntity>> GetAllAsync();
}