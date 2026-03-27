namespace HortaGestao.Application.DTOs.Request;

public class ImportMessagingDto
{
    public Guid ImportId  { get; set; }
    public Guid SellerId { get; set; }
    public string Timestamp { get; set; }
    public ProductCreateDto Product { get; set; }
    public int Quantity { get; set; }
    public string Line { get; set; }
}