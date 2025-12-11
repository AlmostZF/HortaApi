
namespace DDDPractice.Domain.Entities;

public abstract class SystemUserEntity
{
    public Guid Id { get; protected set; }
    public string Name { get; protected set; }
    public string PhoneNumber { get; protected set; }
    
    protected SystemUserEntity(){}

    protected SystemUserEntity(string name, string phoneNumber)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
    }
    
    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        Name = name;
    }

    public void SetPhoneNumber(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be empty");

        PhoneNumber = phone;
    }
    
}