namespace HortaGestao.Application.DTOs.Response;

public class LastReservationResponseDto

{
    public Guid ReservationId { get; set; }
    public string CustomerName { get; set; }
    public string OrderStatus { get; set; }
    public decimal TotalValue { get; set; }
    public int ItemsCount { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime PickUpDate { get; set; }
    public DateTime PickupDeadline { get; set; }
    
}

public class ProductReservationSummaryResponseDto
{
    public string ProductName { get; set; }
    public decimal ValueTotal { get; set; }
    public decimal LastUnitPriceSold { get; set; }
    public decimal CurrentUnitPrice { get; set; }
    public int Quantity { get; set; }
}