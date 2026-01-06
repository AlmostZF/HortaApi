namespace HortaGestao.Application.DTOs.Request;

public class OrderReservationUpdateDto
{
    public Guid Id { get; set; }
    public string SecurityCode { get; set; }
    
    public Guid UserId { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime PickupDate { get; set; }
    public DateTime PickupDeadline { get; set; }
    
    public PickupLocationUpdateDto PickupLocation { get; set; }
    public decimal ReservationFee { get; set; }
    public string OrderStatus { get; set; }
    public decimal ValueTotal { get; set; }
    
    public IEnumerable<OrderReservationItemUpdateDto> listOrderItens { get; set; }
    
}