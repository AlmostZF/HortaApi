using DDDPractice.DDDPractice.Domain.Entities;
using DDDPractice.DDDPractice.Domain.ValueObjects;
using DDDPractice.DDDPractice.Domain;
using DDDPractice.DDDPractice.Domain.Enums;
using DDDPractice.DDDPractice.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DDDPractice.DDDPractice.Infrastructure.Repositories;

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
            .Where(o => o.SecurityCode.Value == securityCode)
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