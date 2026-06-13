namespace HortaGestao.Application.DTOs.Request;

public class ImportMessagingDto
{
    public Guid ImportId  { get; set; }
    public Guid UserId { get; set; }
    public string Timestamp { get; set; }
    public List<ProductCreateDto> Products { get; set; }
    public int Quantity { get; set; }
    public string Line { get; set; }
    public int TotalMessages { get; set; }
}