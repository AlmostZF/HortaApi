using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Infrastructure.Interfaces;

namespace HortaGestao.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardQueries _dashboardQueries;

    public DashboardService(IDashboardQueries dashboardQueries)
    {
        _dashboardQueries = dashboardQueries;
    }

    public async Task<DashboardResponseDto> GetFullDashboardAsync(Guid sellerId, int month, int year, int limit = 10)
    {
        var summaryTask = await _dashboardQueries.GetGeneralSummary(sellerId, month, year);
        var lastReservationTask = await _dashboardQueries.GetLastReservations(sellerId, limit);
        var topSellingProductTask = await _dashboardQueries.GetTopSellingProducts(sellerId, month, year);
        var yearEvolutioonTask = await _dashboardQueries.GetYearlyEvolution(sellerId, year);

        return new DashboardResponseDto()
        {
            RecentReservations = lastReservationTask,
            Summary = summaryTask,
            TopProducts = topSellingProductTask,
            YearlyReport = yearEvolutioonTask
        };

    }
}