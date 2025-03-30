using RiPOS.Database;

namespace RiPOS.Repository.Session
{
    public class RepositorySessionFactory(RiPosDbContext context) : IRepositorySessionFactory
    {
        public RepositorySession Create()
        {
            var dbSession = new RepositorySession(context);
            return dbSession;
        }
    }
}
