using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;

namespace HortaGestao.Application.Services;

public class DashboardService:IDashboardService
{
    public Task<SellerSummaryResponseDto> GetGeneralSummary(Guid sellerId, int month, int year)
    {
        throw new NotImplementedException();
    }

    public Task<YearlyReportResponseDto> GetYearlyEvolution(Guid sellerId, int year)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LastReservationResponseDto>> GetLastReservations(Guid sellerId, int limit = 10)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TopProductResponseDto>> GetTopSellingProducts(Guid sellerId, int month, int year)
    {
        throw new NotImplementedException();
    }
}