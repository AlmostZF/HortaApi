using System.ComponentModel.DataAnnotations.Schema;

namespace DDD_Practice.DDDPractice.Domain.Entities;

public class StockEntity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
    public DateTime MovementDate { get; set; }
    
    [ForeignKey(nameof(ProductId))]
    public ProductEntity Product { get; set; }
}