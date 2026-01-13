namespace HortaGestao.Application.DTOs.Response;

public class SellerSummaryResponseDto
{
    public decimal TotalProfit { get; set; }
    public int TotalReservations { get; set; }
    public int FinishedReservations { get; set; }
    public int PendingReservations { get; set; } 
    public int CanceledReservations { get; set; }
    public int ExpiredReservations { get; set; }
}