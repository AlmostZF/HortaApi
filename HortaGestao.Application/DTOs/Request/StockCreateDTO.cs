using HortaGestao.Domain.Entities;

namespace HortaGestao.Application.DTOs.Request;

public class StockCreateDto
{
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; }
    public int Quantity { get; set; }
}