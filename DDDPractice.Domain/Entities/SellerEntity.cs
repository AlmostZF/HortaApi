using DDD_Practice.DDDPractice.Domain.ValueObjects;

namespace DDD_Practice.DDDPractice.Domain.Entities;

public class SellerEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public PickupLocation PickupLocation { get; private set; }
    protected SellerEntity() { }
    public SellerEntity(PickupLocation pickupLocation)
    {
        PickupLocation = pickupLocation;
    }
}