using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Domain.Entities;

public class PickupLocationEntity
{
    public Guid Id { get; private set; }
    public Address Address { get; private set; }
    public Guid SellerEntityId { get; private set; }
    
    private readonly List<PickupDay> _availablePickupDays = new();
    public IReadOnlyCollection<PickupDay> AvailablePickupDays => _availablePickupDays.AsReadOnly();
    
    public PickupLocationEntity(){}
    
    public PickupLocationEntity(Address address, IEnumerable<PickupDay> availableDays, Guid sellerEntityId)
    {
        Id = Guid.NewGuid();
        Address = address ?? throw new ArgumentNullException(nameof(address));
        SellerEntityId = sellerEntityId;

        if (!availableDays.Any())
            throw new ArgumentException("Pickup point must have at least one available day.");

        _availablePickupDays.AddRange(availableDays);
    }
    
    public bool IsAvailableOn(DayOfWeek day)
        => _availablePickupDays.Any(d => d.Day == day);

    public void AddDay(PickupDay day)
    {
        if (_availablePickupDays.Any(d => d.Day == day.Day))
            return;

        _availablePickupDays.Add(day);
    }
    
    public void SetAvailableDays(IEnumerable<PickupDay> days)
    {
        _availablePickupDays.Clear();

        if (days == null || !days.Any())
            throw new ArgumentException("Pickup point must have at least one available day.");

        _availablePickupDays.AddRange(days);
    }
    
    public void ChangeAddress(Address newAddress)
    {
        Address = newAddress ?? throw new ArgumentNullException(nameof(newAddress));
    }
    
    public void RemoveDay(DayOfWeek day)
    {
        _availablePickupDays.RemoveAll(d => d.Day == day);
    }

}