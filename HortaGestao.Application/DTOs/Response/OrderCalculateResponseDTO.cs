using System.Text.Json.Serialization;

namespace HortaGestao.Application.DTOs.Response;

public class OrderCalculateResponseDto
{
    [JsonPropertyName("listOrderItens")] 
    public List<OrderReservationItemResponseDto>? ListOrderItens { get; set; }
    public decimal? Fee { get; set; }

    public decimal? Total { get; set; }

}