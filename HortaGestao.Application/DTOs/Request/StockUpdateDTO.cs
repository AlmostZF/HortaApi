namespace HortaGestao.Application.DTOs.Request;

public class StockUpdateDto
{
    public Guid Id { get; set; }
    //public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}