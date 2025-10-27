using System.ComponentModel.DataAnnotations.Schema;
using DDD_Practice.DDDPractice.Domain.Enums;

namespace DDD_Practice.DDDPractice.Domain.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ProductType ProductType { get; set; }
    public decimal UnitPrice { get; set; }
    public Guid SellerId { get; set; }

    public string ConservationDays { get; set; }

    public string Image { get; set; }

    public string ShortDescription { get; set; }
    
    public string LargeDescription { get; set; }
    
    public string Weight { get; set; }
    

    [ForeignKey(nameof(SellerId))]
    public SellerEntity? Seller { get; set; }
}