using DDDPractice.DDDPractice.Domain.Enums;

namespace DDDPractice.DDDPractice.Domain.Repositories;

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