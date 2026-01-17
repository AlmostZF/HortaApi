using HortaGestao.Domain.Entities;

namespace HortaGestao.Domain.ValueObjects;

public class OrderCalculate
{
    public List<OrderReservationItemEntity>? ListOrderItens { get; set; }
    public decimal? Fee { get; set; }

    public decimal? Total { get; set; }
    
}