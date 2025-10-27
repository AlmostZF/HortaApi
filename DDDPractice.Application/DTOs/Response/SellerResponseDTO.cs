using DDD_Practice.DDDPractice.Domain.ValueObjects;

namespace DDDPractice.Application.DTOs;

public class SellerResponseDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public PickupLocation? PickupLocation { get; set; }
}