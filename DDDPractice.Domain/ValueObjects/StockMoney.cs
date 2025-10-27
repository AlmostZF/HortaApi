namespace DDD_Practice.DDDPractice.Domain.ValueObjects;

public class StockMoney
{
    public decimal Amount { get; }
    
    public StockMoney(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        Amount = amount;
    }

    public static StockMoney CalculateTotal(decimal unitPrice, int quantity)
    {
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative.");

        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative.");

        return new StockMoney(unitPrice * quantity);
    }
}