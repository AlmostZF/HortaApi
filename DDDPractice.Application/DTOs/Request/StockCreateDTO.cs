namespace DDDPractice.Application.DTOs.Request.ProductCreateDTO;

public class StockCreateDTO
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}