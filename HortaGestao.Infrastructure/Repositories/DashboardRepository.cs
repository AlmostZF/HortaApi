using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Domain.Entities;
using HortaGestao.Infrastructure.Database.AppDbContext;
using HortaGestao.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HortaGestao.Infrastructure.Repositories;

public class DashboardRepository:IDashboardQueries
{
    private readonly AppDbContext _context;

    public DashboardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SellerSummaryResponseDto> GetGeneralSummary(Guid sellerId, int month, int year)
    {
        var reservationsWithSellerItems = await _context.OrderReservation
            .Include(r=>r.ListOrderItems)
            .Where(r => r.ReservationDate.Month == month &&
                        r.ReservationDate.Year == year &&
                        r.ListOrderItems.Any(i => i.Seller.Id == sellerId))
            .ToListAsync();
        
        return new SellerSummaryResponseDto
        {
            TotalReservations = reservationsWithSellerItems.Count,
            FinishedReservations = reservationsWithSellerItems
                .Count(r => r.OrderStatus == StatusOrder.Confirmada),

            PendingReservations = reservationsWithSellerItems
                .Count(r => r.OrderStatus == StatusOrder.Pendente),

            CanceledReservations = reservationsWithSellerItems
                .Count(r => r.OrderStatus == StatusOrder.Cancelada),

            ExpiredReservations = reservationsWithSellerItems
                .Count(r => r.OrderStatus == StatusOrder.Expirada),

            TotalProfit = reservationsWithSellerItems
                .Where(o => o.OrderStatus == StatusOrder.Confirmada)
                .Sum(r => r.ListOrderItems
                    .Where(i => i.SellerId == sellerId)
                    .Sum(i => i.TotalPrice))
        };
        
    }

    public async Task<YearlyReportResponseDto> GetYearlyEvolution(Guid sellerId, int year)
    {
        var reservations = await _context.OrderReservation
            .Include(r=>r.ListOrderItems)
            .Where(r => r.ReservationDate.Year == year &&
                        r.ListOrderItems.Any(i => i.Seller.Id == sellerId))
            .ToListAsync();

        var salesEvolution = reservations
            .Where(r => r.OrderStatus == StatusOrder.Confirmada)
            .GroupBy(r => r.ReservationDate.Month)
            .Select(g => new ChartDataResponseDto
            {
                Label = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                Quantity = g.Count(),
                Value = g.Sum(r => r.ListOrderItems
                    .Where(i => i.SellerId == sellerId)
                    .Sum(i => i.TotalPrice))
            })
            .OrderBy(x => x.Label)
            .ToList();
        
        var statusComparison = reservations
            .GroupBy(r => r.OrderStatus)
            .Select(g=> new ChartDataResponseDto
            {
                Quantity = g.Count(),
                Value = g.Sum(r => r.ListOrderItems
                    .Where(i => i.SellerId == sellerId)
                    .Sum(i => i.TotalPrice)),
                Label = g.Key.ToString()
            })
            .ToList();
        
        return new YearlyReportResponseDto
        {
            SalesEvolution = salesEvolution,
            StatusComparison = statusComparison
        };
    }

    public async Task<IEnumerable<LastReservationResponseDto>> GetLastReservations(Guid sellerId, int limit = 10)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TopProductResponseDto>> GetTopSellingProducts(Guid sellerId, int month, int year)
    {
        throw new NotImplementedException();
    }
    

}