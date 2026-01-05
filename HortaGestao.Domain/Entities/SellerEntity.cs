using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Domain.Entities;

public class SellerEntity : SystemUserEntity
{
    public PickupLocation PickupLocation { get; private set; }
    protected SellerEntity() { }
    public SellerEntity(string name, string phoneNumber, PickupLocation pickupLocation):base(name, phoneNumber)
    {
        PickupLocation = pickupLocation ?? throw new ArgumentNullException(nameof(pickupLocation));
    }
}