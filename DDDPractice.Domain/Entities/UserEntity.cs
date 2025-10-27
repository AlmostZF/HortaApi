using DDD_Practice.DDDPractice.Domain.ValueObjects;

namespace DDD_Practice.DDDPractice.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public SecurityCode SecurityCode { get; private set; }
    protected UserEntity() { }
    public UserEntity(SecurityCode securityCode)
    {
        SecurityCode = securityCode;
    }
    
}