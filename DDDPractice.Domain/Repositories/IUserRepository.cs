using DDD_Practice.DDDPractice.Domain.Entities;

namespace DDD_Practice.DDDPractice.Domain.Repositories;

public interface IUserRepository
{
    Task<UserEntity> GetByIdAsync(Guid id);
    Task CreateAsync(UserEntity user);
    Task UpdateAsync(UserEntity user);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<UserEntity>> GetAllAsync();
}