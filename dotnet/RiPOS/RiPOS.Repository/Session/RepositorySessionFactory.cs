using RiPOS.Database;

namespace RiPOS.Repository.Session
{
    public class RepositorySessionFactory : IRepositorySessionFactory
    {
        private readonly RiPOSDbContext _context;

        public RepositorySessionFactory(RiPOSDbContext context)
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
