using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs.Response;
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
        var reservations = await _context.OrderReservation
            .Where(r => r.SellerId == sellerId)
            .Include(r=>r.ListOrderItems)
            .Where(r => r.ReservationDate.Month == month &&
                        r.ReservationDate.Year == year)
            .ToListAsync();
        
        return new SellerSummaryResponseDto
        {
            TotalReservations = reservations.Count,
            FinishedReservations = reservations
                .Count(r => r.OrderStatus == StatusOrder.Confirmada),

            PendingReservations = reservations
                .Count(r => r.OrderStatus == StatusOrder.Pendente),

            CanceledReservations = reservations
                .Count(r => r.OrderStatus == StatusOrder.Cancelada),

            ExpiredReservations = reservations
                .Count(r => r.OrderStatus == StatusOrder.Expirada),

            TotalProfit = reservations
                .Where(o => o.OrderStatus == StatusOrder.Confirmada)
                .Sum(r => r.ListOrderItems
                    .Sum(i => i.TotalPrice))
        };
        
    }

    public async Task<YearlyReportResponseDto> GetYearlyEvolution(Guid sellerId, int year)
    {
        var reservations = await _context.OrderReservation
            .Where(r => r.SellerId == sellerId)
            .Include(r=>r.ListOrderItems)
            .Where(r => r.ReservationDate.Year == year)
            .ToListAsync();

        var salesEvolution = reservations
            .Where(r => r.OrderStatus == StatusOrder.Confirmada)
            .GroupBy(r => r.ReservationDate.Month)
            .Select(g => new ChartDataResponseDto
            {
                Label = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                Quantity = g.Count(),
                Value = g.Sum(r => r.ListOrderItems
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
        var reservation = _context.OrderReservation
            .Where(o => o.SellerId == sellerId)
            .OrderByDescending(r => r.ReservationDate)
            .Take(limit)
            .Select(r => new LastReservationResponseDto
            {
                CustomerName = r.Customer != null ? r.Customer.Name : r.GuessCustomer.FullName,
                ItemsCount = r.ListOrderItems.Count(),
                OrderStatus = r.OrderStatus.ToString(),
                PickupDeadline = r.PickupDeadline,
                ReservationDate = r.ReservationDate,
                PickUpDate = r.PickupDate,
                ReservationId = r.Id,
                TotalValue = r.TotalValue,
            });

        return reservation;
    }

    public async Task<IEnumerable<TopProductResponseDto>> GetTopSellingProducts(Guid sellerId, int month, int year)
    {
        var reservation = _context.OrderReservation
            .Where(o => o.SellerId == sellerId &&
                        o.OrderStatus == StatusOrder.Confirmada &&
                        o.ReservationDate.Month == month &&
                        o.ReservationDate.Year == year)
            .Include(o => o.ListOrderItems)
            .SelectMany(r => r.ListOrderItems)
            .GroupBy(i=> i.ProductId)
            .Select(g => new TopProductResponseDto
            {
                CategoryName = g.First().Product.ProductType.ToString(),
                ImageUrl = g.First().Product.Image,
                ProductId = g.First().ProductId,
                ProductName = g.First().Product.Name,
                Profit = g.Sum(o=> o.Quantity * o.UnitPrice),
                quantity = g.First().Quantity,
                TotalSold = g.Sum(o=> o.Quantity),
            })
            .OrderByDescending(dto => dto.Profit);
            
        return reservation;
    }
    

}