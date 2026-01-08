namespace HortaGestao.Application.DTOs.Response;

public class StockAvailableResponseDto
{
    public int? StockLimit { get; set; }
    public bool IsActive { get; set; }
    public ProductResponseDto? Product { get; set; }
}