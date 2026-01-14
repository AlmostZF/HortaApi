using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Dashboard;

public class GetDashboardOverviewUseCase
{
    private readonly IDashboardService _dashboardService;

    public GetDashboardOverviewUseCase(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<Result<DashboardResponseDto>> ExecuteAsync(Guid sellerId, int month, int year, int limit)
    {
        try
        {
            var dashboardResponse = await _dashboardService.GetFullDashboardAsync(sellerId, month, year, limit);
            return Result<DashboardResponseDto>.Success(dashboardResponse,200);
        }
        catch (Exception e)
        {
            return Result<DashboardResponseDto>.Failure("Erro", 500);
        }
    }
}