using DDDPractice.DDDPractice.Domain.Enums;

namespace HortaGestao.Application.DTOs.Request;

public class ProductFilterDto
{
    public string? Category { get; set; }
    public string? Name { get; set; }
    public string? Seller { get; set; }
    
    public bool? OrderByLowestPrice { get; set; }
    public int? PageNumber { get; set; } = 1;
    public int? MaxItensPerPage { get; set; } = 10;
}