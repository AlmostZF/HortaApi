namespace HortaGestao.Application.DTOs.Response;

public class MonthlyDetailDto
{
    public string Month { get; set; }
    public decimal TotalValue { get; set; }
    public int MonthNumber { get; set; }
    public List<ChartDataResponseDto> Statuses { get; set; }
}
public class YearlyReportResponseDto
{
    public List<MonthlyDetailDto> MonthlyData { get; set; }
    public List<ChartDataResponseDto> YearlyTotalStatus { get; set; }
}

public class ChartDataResponseDto
{    
    public string Label { get; set; }
    public decimal Value { get; set; }
    public int Quantity { get; set; }
}
