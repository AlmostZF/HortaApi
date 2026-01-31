using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Domain.Entities;

namespace HortaGestao.Domain.IRepositories;

public interface IOrderReservationRepository
{
    Task<OrderReservationEntity> GetBySecurityCodeAsync(string securityCode, Guid sellerId);
    Task<OrderReservationEntity> GetBySellerIdAsync(Guid id);
    Task<OrderReservationEntity> GetByIdAsync(Guid id, Guid sellerId);
    Task UpdateStatusAsync(string status, Guid id);
    Task UpdateAsync(OrderReservationEntity order);
    Task DeleteAsync(Guid id);
    Task AddAsync(OrderReservationEntity order);
    Task<IEnumerable<OrderReservationEntity>> GetByStatusAsync(StatusOrder status);
    Task<IEnumerable<OrderReservationEntity>> GetAllAsync();
}