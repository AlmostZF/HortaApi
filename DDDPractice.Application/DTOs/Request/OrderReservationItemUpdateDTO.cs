namespace DDDPractice.Application.DTOs.Request.ProductCreateDTO;

public class OrderReservationItemUpdateDTO
{
    public Guid ProductId { get; set; }
    public Guid SellerId { get; set; }
    public int Quantity { get; set; }

}