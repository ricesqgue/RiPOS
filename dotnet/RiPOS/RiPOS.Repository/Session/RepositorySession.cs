using Microsoft.EntityFrameworkCore.Storage;
using RiPOS.Database;

namespace RiPOS.Repository.Session
{
    public class RepositorySession : IRepositorySession, IDisposable
    {
        private readonly RiPOSDbContext _context;
        private IDbContextTransaction _transaction;

        public RepositorySession(RiPOSDbContext context)
        {
            _context = context;
            StartTransaction();
        }

        public RiPOSDbContext DbContext { get { return _context; } }

        public void StartTransaction()
        {
            if (_transaction == null)
            {
                _transaction = _context.Database.BeginTransaction();
            }
        }

        public async Task StartTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
        }

        public void Commit()
        {
            try
            {
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
                await _transaction?.CommitAsync();
            }
            catch
            {
                Rollback();
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
                await _transaction?.RollbackAsync();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            _transaction?.Dispose();
            _transaction = null;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _context.SaveChangesAsync();
        }
    }
}
