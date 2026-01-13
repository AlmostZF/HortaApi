namespace HortaGestao.Application.DTOs.Response;

public class TopProductResponseDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public int TotalSold { get; set; }
    public decimal Profit { get; set; }
    public string ImageUrl { get; set; }
    public int quantity { get; set; }
}