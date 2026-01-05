namespace HortaGestao.Application.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}