using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories
{
    public class CashRegisterRepository : GenericRepository<CashRegister>, ICashRegisterRepository
    {
        public CashRegisterRepository(RiPOSDbContext dbContext) : base(dbContext)
        {
        }
    }
}
