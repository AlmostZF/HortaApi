using DDDPractice.DDDPractice.Domain.Entities;

namespace DDDPractice.DDDPractice.Domain.Repositories;

public interface ICustomerRepository
{
    Task<CustomerEntity> GetByIdAsync(Guid id);
    Task CreateAsync(CustomerEntity user);
    Task UpdateAsync(CustomerEntity user);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<CustomerEntity>> GetAllAsync();
}