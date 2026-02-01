namespace HortaGestao.Application.DTOs.Request;

public class UpdateStatusOrderDto
{
    public Guid Id { get; set; }
    public string? SecurityCode { get; set; }
    public Guid? SellerId { get; set; }
}