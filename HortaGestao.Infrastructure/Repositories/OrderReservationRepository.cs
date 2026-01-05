using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.Repositories;
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
        return await _context.OrderReservation
            .Include(o => o.Customer)
            .Include(o => o.ListOrderItems)
            .ThenInclude(i => i.Seller)
            .Include(o => o.ListOrderItems)
            .ThenInclude(i => i.Product)
            .Where(o => 
                o.SecurityCode == new SecurityCode(securityCode) || 
                (o.Customer != null && o.Customer.SecurityCode == new SecurityCode(securityCode))
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
            .FirstOrDefaultAsync(o => o.Id == id);
        
        if (order == null)
            throw new Exception("Order not found.");
        
        return order;
    }
    

    public async Task UpdateStatusAsync(StatusOrder status, Guid id)
    {
        var order = await _context.OrderReservation.FindAsync(id);
        if (order == null)
            throw new Exception("Order not found.");
        
        order.OrderStatus = status;
        _context.Update(order);
        await _context.SaveChangesAsync();
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
            .ToListAsync();
    }
    
}