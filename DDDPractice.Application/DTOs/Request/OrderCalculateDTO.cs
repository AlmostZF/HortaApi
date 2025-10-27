using System.Text.Json.Serialization;

namespace DDDPractice.Application.DTOs.Request.ProductCreateDTO;

public class OrderCalculateDTO
{
    [JsonPropertyName("listOrderItens")] 
    public List<OrderReservationItemCreateDTO> listOrderItens { get; set; }

}