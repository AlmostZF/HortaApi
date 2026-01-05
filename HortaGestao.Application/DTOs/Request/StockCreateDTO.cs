namespace HortaGestao.Application.DTOs.Request;

public class StockCreateDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}