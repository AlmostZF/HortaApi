
using HortaGestao.Application.DTOs.Response;

namespace HortaGestao.Application.Interfaces.Services;

public interface IDashboardService
{
    public Task<DashboardResponseDto> GetFullDashboardAsync(Guid sellerId, int month, int year, int limit = 10);
}