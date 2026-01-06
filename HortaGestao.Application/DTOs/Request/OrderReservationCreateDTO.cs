using System.Text.Json.Serialization;
using HortaGestao.Application.DTOs.Response;

namespace HortaGestao.Application.DTOs.Request;

public class OrderReservationCreateDto
{
    
    public Guid? UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime PickupDate { get; set; }
    public DateTime PickupDeadline { get; set; }
    
    public PickupLocationCreateDto PickupLocation { get; set; }
    
    public string OrderStatus { get; set; }
    
    [JsonPropertyName("listOrderItens")] 
    public List<OrderReservationItemCreateDto> listOrderItens { get; set; }
    

}