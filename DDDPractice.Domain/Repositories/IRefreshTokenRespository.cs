using DDDPractice.Domain.Entities;

namespace DDDPractice.DDDPractice.Domain.Repositories;

public interface IRefreshTokenRespository
{
    Task CreateAsync(RefreshTokenEntity refreshToken);
    Task<RefreshTokenEntity?> GetByTokenAsync(string Token);
    Task UpdateAsync(RefreshTokenEntity refreshToken);
    Task<bool> DeleteAsync(string refreshToken);

}