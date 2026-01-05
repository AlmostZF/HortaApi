using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Application.DTOs.Response;

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