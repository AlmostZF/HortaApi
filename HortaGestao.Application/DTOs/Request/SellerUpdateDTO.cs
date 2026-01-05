using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Application.DTOs.Request;

public class SellerUpdateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public PickupLocation PickupLocation { get; private set; }
    
    public SellerUpdateDto(PickupLocation pickupLocation)
    {
        PickupLocation = pickupLocation;
    }
}