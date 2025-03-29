using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories
{
    public class VendorRepository(RiPosDbContext dbContext) : GenericRepository<Vendor>(dbContext), IVendorRepository;
}
