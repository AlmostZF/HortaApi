using DDDPractice.Domain.Entities;

namespace DDDPractice.DDDPractice.Domain.Repositories;

public interface ISystemUserRespository
{
    Task<SystemUserEntity> GetByIdAsync(Guid id);
}