using DDD_Practice.DDDPractice.Domain.ValueObjects;

namespace DDDPractice.Application.DTOs.Request.ProductCreateDTO;

public class SellerUpdateDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public PickupLocation PickupLocation { get; private set; }
    
    public SellerUpdateDTO(PickupLocation pickupLocation)
    {
        PickupLocation = pickupLocation;
    }
}