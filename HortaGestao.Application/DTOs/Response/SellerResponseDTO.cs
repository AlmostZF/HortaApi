using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Application.DTOs.Response;

public class SellerResponseDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public PickupLocation? PickupLocation { get; set; }
}