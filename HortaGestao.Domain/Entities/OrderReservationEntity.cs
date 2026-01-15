using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Domain.Entities;

public class OrderReservationEntity
{
    private readonly List<OrderReservationItemEntity> _listOrderItems = new();
    public IReadOnlyCollection<OrderReservationItemEntity> ListOrderItems => _listOrderItems;
    public Guid Id { get; private set; }
    public Guid? CustomerId { get; private set; }
    public virtual CustomerEntity? Customer { get; private set; }
    public Guid SellerId { get; private set; }
    public virtual SellerEntity Seller { get; private set; }
    public GuessCustomer? GuessCustomer { get; private set;  }
    public SecurityCode? SecurityCode { get; private set;  }
    public DateTime PickupDate { get; private set;  }
    public DateTime PickupDeadline { get; private set; }
    public decimal ReservationFee { get; private set;  }
    public StatusOrder OrderStatus { get; private set;  }
    public DateTime ReservationDate { get; private set;  }
    public Guid PickupLocationId { get; private set; }
    public virtual PickupLocationEntity PickupLocation { get; private set; }
    public decimal TotalValue { get; private set;  }
    
    protected OrderReservationEntity() { }

    public static OrderReservationEntity CreateForCustomer(Guid customerId, PickupLocationEntity location, DateTime pickupDate, DateTime deadline, decimal fee,Guid sellerId)
    {
        var order = new OrderReservationEntity(location, pickupDate, deadline, fee,sellerId);
        order.CustomerId = customerId;
        return order;
    }
    
    public static OrderReservationEntity CreateForGuest(string name, string email, string phone, PickupLocationEntity location, DateTime pickupDate, DateTime deadline, decimal fee,Guid sellerId)
    {
        var order = new OrderReservationEntity(location, pickupDate, deadline, fee,sellerId);
        order.GuessCustomer = new GuessCustomer(name, email, phone);
        order.SecurityCode = new SecurityCode(order.GenerateSecurityCode());
        return order;
    }
    
    private OrderReservationEntity(PickupLocationEntity location, DateTime pickupDate, DateTime deadline, decimal fee, Guid sellerId)
    {
        Id = Guid.NewGuid();
        OrderStatus = StatusOrder.Pendente;
        ReservationDate = DateTime.UtcNow;
        PickupLocation = location;
        PickupLocationId = location.Id;
        ReservationFee = fee;
        SellerId = sellerId;
        
        
        if (deadline < pickupDate) throw new ArgumentException("Deadline mismatch.");
        PickupDate = pickupDate;
        PickupDeadline = deadline;
    }

    public void AddItem(Guid productId, Guid sellerId, int quantity, decimal unitPrice)
    {
        if (OrderStatus != StatusOrder.Pendente) throw new InvalidOperationException("Order is not pending.");
        
        var existing = _listOrderItems.FirstOrDefault(i => i.ProductId == productId);
        
        if (existing != null) existing.IncreaseQuantity(quantity);
        else _listOrderItems.Add(new OrderReservationItemEntity(Id, productId, sellerId, quantity, unitPrice));

        CalculateTotal();
    }
    
    public string GetOrderString()
    {
        return OrderStatus.ToString();
    }

    public void UpdateOrderStatus(StatusOrder newStatus)
    {
        OrderStatus = newStatus;
    }

    private void CalculateTotal() => TotalValue = _listOrderItems.Sum(i => i.TotalPrice) + ReservationFee;

    private string GenerateSecurityCode() => Guid.NewGuid().ToString()[..4].ToUpper();
}