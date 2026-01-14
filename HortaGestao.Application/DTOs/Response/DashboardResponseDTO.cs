namespace HortaGestao.Application.DTOs.Response;

public class DashboardResponseDto
{
    public SellerSummaryResponseDto Summary { get; set; }
    public YearlyReportResponseDto YearlyReport { get; set; }
    public IEnumerable<LastReservationResponseDto> RecentReservations { get; set; }
    public IEnumerable<TopProductResponseDto> TopProducts { get; set; }
}