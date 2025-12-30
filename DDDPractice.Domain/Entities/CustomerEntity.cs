using DDDPractice.DDDPractice.Domain.ValueObjects;
using DDDPractice.Domain.Entities;

namespace DDDPractice.DDDPractice.Domain.Entities;

public class CustomerEntity: SystemUserEntity
{
    public SecurityCode SecurityCode { get; private set; }
    protected CustomerEntity() { }
    public CustomerEntity(string? name, string? phoneNumber, SecurityCode securityCode):base(name, phoneNumber)
    {
        SecurityCode = securityCode ?? throw new ArgumentNullException(nameof(securityCode));
    }
    
}