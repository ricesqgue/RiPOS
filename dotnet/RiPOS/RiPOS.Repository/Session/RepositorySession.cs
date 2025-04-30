using Microsoft.EntityFrameworkCore.Storage;
using RiPOS.Database;

namespace RiPOS.Repository.Session;

public class RepositorySession(RiPosDbContext context) : IRepositorySession, IDisposable
{
    private IDbContextTransaction? _transaction = null;

    public RiPosDbContext DbContext => context;

    public void StartTransaction()
    {
        if (_transaction == null)
        {
            _transaction = context.Database.BeginTransaction();
        }
    }

    public async Task StartTransactionAsync()
    {
        if (_transaction == null)
        {
            _transaction = await context.Database.BeginTransactionAsync();
        }
    }

    public void Commit()
    {
        try
        {
            SaveChanges();
            _transaction?.Commit();
        }
        catch
        {
            Rollback();
            throw;
        }
    }

    public async Task CommitAsync()
    {
        try
        {
            await SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
    }

    public void Rollback () 
    {
        try
        {
            _transaction?.Rollback();
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        context.Dispose();
        _transaction?.Dispose();
        _transaction = null;
    }

    public int SaveChanges()
    {
        return context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }
}