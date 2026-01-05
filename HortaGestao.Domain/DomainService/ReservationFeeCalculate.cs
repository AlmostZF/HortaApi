namespace HortaGestao.Domain.DomainService;

public class ReservationFeeCalculate : IReservationFeeCalculate
{
    public decimal CalculateFeeCalculate(decimal valueTotal)
    {
        return valueTotal * 0.10m;
    }
}