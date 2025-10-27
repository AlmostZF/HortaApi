using DDD_Practice.DDDPractice.Domain.Entities;
using DDD_Practice.DDDPractice.Domain.Enums;
using DDD_Practice.DDDPractice.Domain.ValueObjects;

namespace DDD_Practice.DDDPractice.Domain.Repositories;

public interface IOrderReservationRepository
{
    Task<IEnumerable<OrderReservationEntity>> GetBySecurityCodeAsync(string securityCode);
    Task<OrderReservationEntity> GetByIdAsync(Guid id);
    Task UpdateStatusAsync(StatusOrder status, Guid id);
    Task UpdateAsync(OrderReservationEntity order);
    Task DeleteAsync(Guid id);
    Task AddAsync(OrderReservationEntity order);
    Task<IEnumerable<OrderReservationEntity>> GetByStatusAsync(StatusOrder status);
    Task<IEnumerable<OrderReservationEntity>> GetAllAsync();
}