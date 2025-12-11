using DDD_Practice.DDDPractice.Domain.ValueObjects;
using DDDPractice.Domain.Entities;

namespace DDD_Practice.DDDPractice.Domain.Entities;

public class CustomerEntity: SystemUserEntity
{
    public SecurityCode SecurityCode { get; private set; }
    protected CustomerEntity() { }
    public CustomerEntity(string? name, string? phoneNumber, SecurityCode securityCode):base(name, phoneNumber)
    {
        SecurityCode = securityCode ?? throw new ArgumentNullException(nameof(securityCode));
    }
    
}