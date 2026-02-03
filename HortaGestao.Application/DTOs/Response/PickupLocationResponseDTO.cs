
namespace HortaGestao.Application.DTOs.Response;

public class PickupLocationResponseDto
{
    public Guid Id { get; set; }
    public string? CustomName { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string Neighborhood { get; set; }
    public string State { get; set; }
    
    public List<DayOfWeek> PickupDays { get; set; }
}