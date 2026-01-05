namespace HortaGestao.Application.DTOs.Request.ProductCreateDTO;

public abstract class OrderReservationItemUpdateDto
{
    public Guid ProductId { get; set; }
    public Guid SellerId { get; set; }
    public int Quantity { get; set; }

}