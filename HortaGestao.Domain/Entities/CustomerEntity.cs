using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Domain.Entities;

public class CustomerEntity: SystemUserEntity
{
    public SecurityCode SecurityCode { get; private set; }
    protected CustomerEntity() { }
    public CustomerEntity(string? name, string? phoneNumber, SecurityCode securityCode):base(name, phoneNumber)
    {
        SecurityCode = securityCode ?? throw new ArgumentNullException(nameof(securityCode));
    }
    
    public string GetSecurityCode()
    {
        return SecurityCode.Value;
    }

    public void UpdateContact(string name, string phoneNumber)
    {
        SetName(name);
        SetPhoneNumber(phoneNumber);
    }

}