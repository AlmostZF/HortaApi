using HortaGestao.Domain.Entities;

namespace HortaGestao.Domain.IRepositories;

public interface IRefreshTokenRespository
{
    Task CreateAsync(RefreshTokenEntity refreshToken);
    Task<RefreshTokenEntity?> GetByTokenAsync(string Token);
    Task UpdateAsync(RefreshTokenEntity refreshToken);
    Task<bool> DeleteAsync(string refreshToken);

}