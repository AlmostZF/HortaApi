
namespace HortaGestao.Application.DTOs.Response;

public class ProductResponseDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string ProductType { get; set; }
    public decimal UnitPrice { get; set; }
    
    public string? ConservationDays { get; set; }

    public string? Image { get; set; }

    public string? ShortDescription { get; set; }
    
    public string? LargeDescription { get; set; }
    
    public string? Weight { get; set; }
    public SellerResponseDto? Seller { get; set; }

}
