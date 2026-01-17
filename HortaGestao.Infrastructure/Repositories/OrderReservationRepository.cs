using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.IRepositories;
using HortaGestao.Domain.ValueObjects;
using HortaGestao.Infrastructure.Database.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace HortaGestao.Infrastructure.Repositories;

public class OrderReservationRepository : IOrderReservationRepository
{
    private readonly AppDbContext _context;

    public OrderReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderReservationEntity>> GetBySecurityCodeAsync(string securityCode)
    {
        var code = new SecurityCode(securityCode);
        
        return await _context.OrderReservation
            .Include(o=> o.Customer)
            .Include(o => o.ListOrderItems)
                .ThenInclude(p=>p.Product)
            .Include(o=> o.PickupLocation)
            .Where(o => o.SecurityCode == code ||
                        (o.Customer!= null && o.SecurityCode == code)
                        )
            .ToListAsync();
    }

    public async Task<OrderReservationEntity> GetByIdAsync(Guid id)
    {
        var order = await _context.OrderReservation
            .Include(o => o.Customer)
            .Include(o => o.ListOrderItems)
                .ThenInclude(i => i.Seller)
            .Include(o => o.ListOrderItems)
                .ThenInclude(i => i.Product)
            .Include(o=> o.PickupLocation)
            .FirstOrDefaultAsync(o => o.SellerId == id);
        
        if (order == null)
            throw new Exception("Order not found.");
        
        return order;
    }
    

    public async Task UpdateStatusAsync(string status, Guid id)
    {
        var order = await _context.OrderReservation.FindAsync(id);
        if (order == null)
            throw new Exception("Order not found.");
        
        if (Enum.TryParse<StatusOrder>(status, ignoreCase: true, out var categoryEnum))
        {
            order.UpdateOrderStatus(categoryEnum);
            _context.Update(order);
            await _context.SaveChangesAsync();
        }

    }

    public async Task UpdateAsync(OrderReservationEntity order)
    {
        
        var existingOrder = await _context.OrderReservation.FindAsync(order.Id);
        if (existingOrder == null)
        {
            throw new InvalidOperationException("Reserva não encontrada.");
        }
        _context.Update(order);
        await _context.SaveChangesAsync();
    }
    

    public async Task DeleteAsync(Guid id)
    {
        var order = await _context.OrderReservation.FindAsync(id);
        if (order == null)
            throw new Exception("Order not found.");

        _context.Remove(order);
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(OrderReservationEntity order)
    {
        _context.OrderReservation.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<OrderReservationEntity>> GetByStatusAsync(StatusOrder status)
    {
        return await _context.OrderReservation
            .Where(o => o.OrderStatus == status)
            .Include(o => o.Customer)
            .Include(o => o.ListOrderItems)
                .ThenInclude(i => i.Seller)
            .Include(o => o.ListOrderItems)
                .ThenInclude(i => i.Product)
            .Include(o=> o.PickupLocation)
            .ToListAsync();
    }

    public async Task<IEnumerable<OrderReservationEntity>> GetAllAsync()
    {
        return await _context.OrderReservation
            .Include(o => o.Customer)
            .Include(o => o.ListOrderItems)
                .ThenInclude(item => item.Product)
            .Include(o => o.ListOrderItems)
                .ThenInclude(item => item.Seller)
            .Include(o=> o.PickupLocation)
            .ToListAsync();
    }
}