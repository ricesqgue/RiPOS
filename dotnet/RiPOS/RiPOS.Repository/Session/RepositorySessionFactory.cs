using RiPOS.Database;

namespace RiPOS.Repository.Session
{
    public class RepositorySessionFactory : IRepositorySessionFactory
    {
        private readonly RiPosDbContext _context;

        public RepositorySessionFactory(RiPosDbContext context)
        {
            _context = context;
        }

        public RepositorySession Create()
        {
            var dbSession = new RepositorySession(_context);
            return dbSession;
        }
    }
}
