namespace HortaGestao.Application.DTOs.Response;

public class StockResponseDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    
    public decimal Total { get; set; }
    
    public DateTime? MovementDate { get; set; }

    public ProductResponseDto? Product { get; set; }
}