
using Microsoft.AspNetCore.Http;

namespace HortaGestao.Application.DTOs.Request;

public class ProductUpdateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ProductType { get; set; }
    public decimal UnitPrice { get; set; }
    public string ConservationDays { get; set; }

    public IFormFile? Image { get; set; }

    public string ShortDescription { get; set; }
    
    public string LargeDescription { get; set; }

    public string Weight { get; set; }
    
    public bool IsActive { get; set; }
}

public class ProductUpdateStatusDto
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
}