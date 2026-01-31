using System.Text.Json.Serialization;

namespace HortaGestao.Application.DTOs.Request;

public class OrderReservationCreateDto
{

    public Guid? UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }

    public List<OrderReservationDetailsDto> OrderDetails { get; set; }
}

public class OrderReservationDetailsDto{
    
    public Guid SellerId { get; set; }
    
    public DateTime PickupDate { get; set; }
    
    public DateTime PickupDeadline { get; set; }
    
    [JsonPropertyName("listOrderItens")] 
    public List<OrderReservationItemDto> listOrderItens { get; set; }
    
    public PickupLocationCreateDto PickupLocation { get; set; }
}