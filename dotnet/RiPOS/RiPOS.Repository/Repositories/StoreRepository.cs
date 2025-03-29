using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories
{
    public class StoreRepository(RiPosDbContext dbContext) : GenericRepository<Store>(dbContext), IStoreRepository;
}
