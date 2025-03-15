using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories
{
    public class SizeRepository : GenericRepository<Size>, ISizeRepository
    {
        public SizeRepository(RiPOSDbContext dbContext) : base(dbContext)
        {
        }
    }
}
