namespace DDD_Practice.DDDPractice.Domain.Service;

public class ReservationFeeCalculate : IReservationFeeCalculate
{
    public decimal CalculateFeeCalculate(decimal valueTotal)
    {
        return valueTotal * 0.10m;
    }
}