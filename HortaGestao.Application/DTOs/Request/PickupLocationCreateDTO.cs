namespace HortaGestao.Application.DTOs.Request;

public class PickupLocationCreateDto
{
    public Guid SellerId { get; set; }
    public Guid? Id { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string Neighborhood { get; set; }
    public string State { get; set; }
    public List<DayOfWeek> PickupDays { get; set; }
}