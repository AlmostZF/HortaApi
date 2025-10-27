using DDD_Practice.DDDPractice.Domain.Enums;
using DDD_Practice.DDDPractice.Domain.ValueObjects;

namespace DDDPractice.Application.DTOs.Request.ProductCreateDTO;

public class OrderReservationUpdateDTO
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
    
    public IEnumerable<OrderReservationItemUpdateDTO> listOrderItens { get; set; }
    
}