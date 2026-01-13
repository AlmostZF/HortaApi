using HortaGestao.Application.DTOs.Response;

namespace HortaGestao.Infrastructure.Interfaces;

public interface IDashboardQueries
{
    public Task<SellerSummaryResponseDto> GetGeneralSummary(Guid sellerId, int month, int year);
    public Task<YearlyReportResponseDto> GetYearlyEvolution(Guid sellerId, int year);
    public Task<IEnumerable<LastReservationResponseDto>> GetLastReservations(Guid sellerId, int limit = 10);
    public Task<IEnumerable<TopProductResponseDto>> GetTopSellingProducts(Guid sellerId, int month, int year);
}