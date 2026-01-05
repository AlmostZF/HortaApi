using System.Text.Json.Serialization;

namespace HortaGestao.Application.DTOs.Request;

public class OrderCalculateDto
{
    [JsonPropertyName("listOrderItens")] 
    public List<OrderReservationItemCreateDto> listOrderItens { get; set; }

}