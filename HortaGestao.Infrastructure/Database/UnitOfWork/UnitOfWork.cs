using HortaGestao.Application.Interfaces;
using HortaGestao.Application.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;

namespace HortaGestao.Infrastructure.Database.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext.AppDbContext _context;
    private IDbContextTransaction? _transaction;
    
    public UnitOfWork(AppDbContext.AppDbContext context)
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