using System.Text.Json.Serialization;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;

namespace DDDPractice.Application.DTOs;

public class OrderCalculateResponseDto
{
    [JsonPropertyName("listOrderItens")] 
    public List<OrderReservationItemResponseDto>? ListOrderItens { get; set; }
    public decimal? Fee { get; set; }

    public decimal? Total { get; set; }

}