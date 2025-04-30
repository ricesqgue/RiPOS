namespace RiPOS.Repository.Session;

public interface IRepositorySession : IDisposable
{
    void StartTransaction();
    Task StartTransactionAsync();
    void Commit();

    Task CommitAsync();

    void Rollback();

    Task RollbackAsync();

    int SaveChanges();

    Task<int> SaveChangesAsync();
}