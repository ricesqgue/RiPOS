using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories
{
    public class SizeRepository(RiPosDbContext dbContext) : GenericRepository<Size>(dbContext), ISizeRepository;
}
