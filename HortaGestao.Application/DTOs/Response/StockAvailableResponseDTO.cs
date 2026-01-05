namespace HortaGestao.Application.DTOs.Response;

public class StockAvailableResponseDto
{
    public int? StockLimit { get; set; }
    public ProductResponseDto? Product { get; set; }
}