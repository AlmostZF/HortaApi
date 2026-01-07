using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Domain.Entities;

public class SellerEntity : SystemUserEntity
{
    private readonly List<PickupLocationEntity> _pickupLocations = new();
    public IReadOnlyCollection<PickupLocationEntity> PickupLocations => _pickupLocations.AsReadOnly();
    
    private SellerEntity() { }
    public SellerEntity(string name, string phoneNumber):base(name, phoneNumber)
    {
    }
    
    public void AddPickupLocation(PickupLocationEntity pickupLocationEntity)
    {
        if (pickupLocationEntity == null)
            throw new ArgumentNullException(nameof(pickupLocationEntity));

        _pickupLocations.Add(pickupLocationEntity);
    }
    
    public void UpdateContact(string name, string phoneNumber)
    {
        SetName(name);
        SetPhoneNumber(phoneNumber);
    }


}