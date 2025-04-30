namespace RiPOS.Repository.Session;

public interface IRepositorySessionFactory
{
    Task<IRepositorySession> CreateAsync(bool startTransaction = true);
    
    IRepositorySession Create(bool startTransaction = true);

}