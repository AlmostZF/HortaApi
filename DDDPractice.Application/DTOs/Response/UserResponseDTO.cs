using DDD_Practice.DDDPractice.Domain.ValueObjects;

namespace DDDPractice.Application.DTOs;

public class UserResponseDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    
    public SecurityCode? SecurityCode { get; private set; }
    
    public UserResponseDto(SecurityCode securityCode)
    {
        SecurityCode = securityCode;
    }
    
}