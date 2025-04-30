using RiPOS.Database;

namespace RiPOS.Repository.Session;

public class RepositorySessionFactory(RiPosDbContext context) : IRepositorySessionFactory
{
    public async Task<IRepositorySession> CreateAsync(bool startTransaction = true)
    {
        var dbSession = new RepositorySession(context);
        if (startTransaction)
        {
            await dbSession.StartTransactionAsync();
        }
        return dbSession;
    }
    
    public IRepositorySession Create(bool startTransaction = true)
    {
        var dbSession = new RepositorySession(context);
        if (startTransaction)
        {
            dbSession.StartTransaction();
        }
        return dbSession;
    }
}