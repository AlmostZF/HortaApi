using DDDPractice.DDDPractice.Domain.Enums;

namespace HortaGestao.Domain.ValueObjects;

public class ProductFilter
{
    public string? Category { get; private set; }
    public string? Name { get; private set; }
    public string? Seller { get; private set; }
    public int PageNumber { get; private set; } = 1;
    public int MaxItensPerPage { get; private set; } = 10;

    public ProductFilter(
        string? category,
        string? name,
        string? seller,
        int? pageNumber,
        int? maxItensPerPage
    )
    {
        Category = category;
        Name = name;
        Seller = seller;
        PageNumber = pageNumber ?? 1;
        MaxItensPerPage = maxItensPerPage ?? 10;
    }
}