namespace HortaGestao.Application.DTOs.Request;

public class OrderReservationItemUpdateDto
{
    public Guid ProductId { get; set; }
    public Guid SellerId { get; set; }
    public int Quantity { get; set; }

}