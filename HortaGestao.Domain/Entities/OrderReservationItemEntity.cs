
namespace HortaGestao.Domain.Entities;

public class OrderReservationItemEntity
{
    public Guid Id { get; private set; }
    public Guid ReservationId { get; private set; }
    public Guid ProductId { get; private set; }
    public virtual ProductEntity Product { get; private set; }
    public Guid SellerId { get; private set;}
    public virtual SellerEntity Seller { get; private set; }
    public int Quantity { get; private set;}
    public decimal UnitPrice { get; private set;}
    
    public decimal TotalPrice => UnitPrice * Quantity;
    
    protected OrderReservationItemEntity() { } 
    
    public OrderReservationItemEntity(Guid reservationId,Guid productId, Guid sellerId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        if (unitPrice <= 0)
            throw new ArgumentException("Unit price must be greater than zero.");

        Id = Guid.NewGuid();
        ReservationId = reservationId;
        ProductId = productId;
        SellerId = sellerId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        Quantity = quantity;
    }
    internal void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        Quantity = newQuantity;
    }
    
    
}