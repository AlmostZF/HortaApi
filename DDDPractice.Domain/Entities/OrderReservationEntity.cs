using System.ComponentModel.DataAnnotations.Schema;
using DDD_Practice.DDDPractice.Domain.Entities;
using DDD_Practice.DDDPractice.Domain.Enums;
using DDD_Practice.DDDPractice.Domain.ValueObjects;
using DDDPractice.Domain.Entities;

namespace DDD_Practice.DDDPractice.Domain;

public class OrderReservationEntity
{
    public Guid Id { get; set; }
    [NotMapped]
    public SecurityCode SecurityCode { get; set; }
    public Guid UserId { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime PickupDate { get; set; }
    public DateTime PickupDeadline { get; set; }
    public PickupLocation PickupLocation { get; set; }
    public decimal ReservationFee { get; set; }
    public StatusOrder OrderStatus { get; set; }
    public decimal ValueTotal { get; set; }

    public ICollection<OrderReservationItemEntity> ListOrderItems { get; set; } = new List<OrderReservationItemEntity>();
    
    [ForeignKey(nameof(UserId))]
    public CustomerEntity Customer { get; set; }
}



