namespace HortaGestao.Application.DTOs.Response;

public class OrderReservationResponseDto
{
    public static OrderReservationResponseDto Empty => new OrderReservationResponseDto();
    public Guid? Id { get; set; }

    public DateTime? ReservationDate { get; set; }
    public DateTime? PickupDate { get; set; }
    public DateTime? PickupDeadline { get; set; }
    public PickupLocationResponseDto? PickupLocation { get; set; }
    public decimal? ReservationFee { get; set; }
    public string? OrderStatus { get; set; }
    public decimal? ValueTotal { get; set; }
    
    public IEnumerable<OrderReservationItemResponseDto?> listOrderItens { get; set; }

    public CustomerResponseDto? UserResponse { get; set; }

}

public class CreateOrderReservationResponseDto
{
    public string SellerName { get; set; }
    public string SecurityCode { get; set; }
}