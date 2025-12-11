using DDD_Practice.DDDPractice.Domain.ValueObjects;

namespace DDDPractice.Application.DTOs;

public class CustomerResponseDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    
    public SecurityCode? SecurityCode { get; private set; }
    
    public CustomerResponseDto(SecurityCode securityCode)
    {
        SecurityCode = securityCode;
    }
    
}