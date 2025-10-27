using DDD_Practice.DDDPractice.Domain.ValueObjects;

namespace DDDPractice.Application.DTOs.Request.ProductCreateDTO;

public class SellerCreateDTO
{

    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public PickupLocation PickupLocation { get; private set; }
    
    public SellerCreateDTO(PickupLocation pickupLocation)
    {
        PickupLocation = pickupLocation;
    }
}