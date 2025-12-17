using DDDPractice.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace DDD_Practice.DDDPractice.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;
    
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await _transaction!.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        await _transaction!.RollbackAsync();
    }
}