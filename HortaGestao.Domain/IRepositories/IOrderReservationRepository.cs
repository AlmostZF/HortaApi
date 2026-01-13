using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Domain.Entities;

namespace HortaGestao.Domain.IRepositories;

public interface IOrderReservationRepository
{
    Task<IEnumerable<OrderReservationEntity>> GetBySecurityCodeAsync(string securityCode);
    Task<OrderReservationEntity> GetByIdAsync(Guid id);
    Task UpdateStatusAsync(string status, Guid id);
    Task UpdateAsync(OrderReservationEntity order);
    Task DeleteAsync(Guid id);
    Task AddAsync(OrderReservationEntity order);
    Task<IEnumerable<OrderReservationEntity>> GetByStatusAsync(StatusOrder status);
    Task<IEnumerable<OrderReservationEntity>> GetAllAsync();
}