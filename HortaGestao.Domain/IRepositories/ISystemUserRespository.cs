using HortaGestao.Domain.Entities;

namespace HortaGestao.Domain.IRepositories;

public interface ISystemUserRespository
{
    Task<SystemUserEntity> GetByIdAsync(Guid id);
}