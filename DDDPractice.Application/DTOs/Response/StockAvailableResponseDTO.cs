namespace DDDPractice.Application.DTOs;

public class StockAvailableResponseDto
{
    public int? StockLimit { get; set; }
    public ProductResponseDto? Product { get; set; }
}