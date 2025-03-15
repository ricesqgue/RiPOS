using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories
{
    public class VendorRepository : GenericRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(RiPOSDbContext dbContext) : base(dbContext)
        {
        }
    }
}
