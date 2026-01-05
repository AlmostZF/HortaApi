using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs.Request.ProductCreateDTO;
using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Application.DTOs.Request;

public class OrderReservationUpdateDto
{
    public Guid Id { get; set; }
    public SecurityCode? SecurityCode { get; set; }
    
    public Guid UserId { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime PickupDate { get; set; }
    public DateTime PickupDeadline { get; set; }
    
    public PickupLocation PickupLocation { get; set; }
    public decimal ReservationFee { get; set; }
    public StatusOrder OrderStatus { get; set; }
    public decimal ValueTotal { get; set; }
    
    public IEnumerable<OrderReservationItemUpdateDto> listOrderItens { get; set; }
    
}