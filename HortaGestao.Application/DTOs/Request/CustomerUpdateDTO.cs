namespace HortaGestao.Application.DTOs.Request;

public class CustomerUpdateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
}