using DDD_Practice.DDDPractice.Domain.ValueObjects;
using DDDPractice.Domain.Entities;

namespace DDD_Practice.DDDPractice.Domain.Entities;

public class SellerEntity : SystemUserEntity
{
    public PickupLocation PickupLocation { get; private set; }
    protected SellerEntity() { }
    public SellerEntity(string name, string phoneNumber, PickupLocation pickupLocation):base(name, phoneNumber)
    {
        PickupLocation = pickupLocation ?? throw new ArgumentNullException(nameof(pickupLocation));
    }
}