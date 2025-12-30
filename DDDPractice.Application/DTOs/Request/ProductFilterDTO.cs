using DDDPractice.DDDPractice.Domain.Enums;

namespace DDDPractice.Application.DTOs.Request.ProductCreateDTO;

public class ProductFilterDTO
{
    public ProductType? Category { get; set; }
    public string? Name { get; set; }
    public string? Seller { get; set; }
    public int? PageNumber { get; set; } = 1;
    public int? MaxItensPerPage { get; set; } = 10;
}