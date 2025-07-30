using Microsoft.EntityFrameworkCore.Storage;
using Shortha.Domain.Interfaces;

namespace Shortha.Infrastructre.Units;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDb _context;
    private IDbContextTransaction _transaction;

    public EfUnitOfWork(AppDb context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        await _transaction.RollbackAsync();
    }
}