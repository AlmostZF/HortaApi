namespace HortaGestao.Application.DTOs.Response;

public class CustomerResponseDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    
    public string SecurityCode { get; set; }
    
}