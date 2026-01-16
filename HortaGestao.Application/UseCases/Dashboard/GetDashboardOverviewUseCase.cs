using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Dashboard;

public class GetDashboardOverviewUseCase
{
    private readonly IDashboardService _dashboardService;
    private readonly IAuthRepository _authRepository;

    public GetDashboardOverviewUseCase(IDashboardService dashboardService,
        IAuthRepository authRepository)
    {
        _dashboardService = dashboardService;
        _authRepository = authRepository;
    }

    public async Task<Result<DashboardResponseDto>> ExecuteAsync(Guid identityId, int month, int year, int limit)
    {
        try
        {
            var sellerId = await _authRepository.GetBusinessIdByIdentityIdAsync(identityId);
            if (sellerId == null)
                return Result<DashboardResponseDto>.Failure("Usuário não encontrado.", 404);
            
            var dashboardResponse = await _dashboardService.GetFullDashboardAsync(sellerId!.Value, month, year, limit);
            return Result<DashboardResponseDto>.Success(dashboardResponse,200);
        }
        catch (Exception e)
        {
            return Result<DashboardResponseDto>.Failure("Erro", 500);
        }
    }
}