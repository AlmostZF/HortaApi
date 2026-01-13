namespace HortaGestao.Application.DTOs.Response;

public class YearlyReportResponseDto
{
    public List<ChartDataResponseDto> SalesEvolution { get; set; }
    public List<ChartDataResponseDto> StatusComparison { get; set; }
}

public class ChartDataResponseDto
{
    public string Label { get; set; }
    public decimal Value { get; set; }
    public int Quantity { get; set; }
}