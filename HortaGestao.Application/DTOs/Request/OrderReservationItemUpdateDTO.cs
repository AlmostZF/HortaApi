namespace HortaGestao.Application.DTOs.Request;

public abstract class OrderReservationItemUpdateDto
{
    public Guid ProductId { get; set; }
    public Guid SellerId { get; set; }
    public int Quantity { get; set; }

}