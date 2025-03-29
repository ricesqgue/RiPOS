using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories
{
    public class CashRegisterRepository(RiPosDbContext dbContext)
        : GenericRepository<CashRegister>(dbContext), ICashRegisterRepository;
}
