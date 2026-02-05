
using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Domain.Entities;

public class StockEntity
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public virtual ProductEntity Product { get; private set; }
    public int Quantity { get; private set; }
    public decimal Total { get; private set; }
    public DateTime MovementDate { get; private set; }
    
    protected StockEntity() { }
    public StockEntity(Guid productId, int initialQuantity, decimal unitPrice)
    {
        if (initialQuantity < 0) throw new ArgumentException("Initial stock quantity cannot be negative.");
        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = initialQuantity;
        MovementDate = DateTime.UtcNow;
        CalculateTotal(unitPrice, initialQuantity);
    }

    public void UpdateStock(Guid productId, decimal unitPrice, int quantity)
    {
        ProductId = productId;
        editQuantity(quantity);
        CalculateTotal(unitPrice, quantity);
    }

    public void CalculateTotal(decimal unitPrice, int quantity)
    {
        Total = StockMoney.CalculateTotal(unitPrice,quantity).Amount;
    }

    public void editQuantity(int amount)
    {
        Quantity = amount;
    }

    public void AddQuantity(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount to increase must be positive.");

        Quantity += amount;
        MovementDate = DateTime.UtcNow;
    }

    public void RemoveQuantity(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount to decrease must be positive.");

        if (Quantity - amount < 0)
            throw new InvalidOperationException("Insufficient stock balance for this operation.");

        Quantity -= amount;
        MovementDate = DateTime.UtcNow;
    }
}