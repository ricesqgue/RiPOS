namespace RiPOS.Repository.Session;

public interface IRepositorySession
{

    void Commit();

    Task CommitAsync();

    void Rollback();

    Task RollbackAsync();

    int SaveChanges();

    Task<int> SaveChangesAsync();
}