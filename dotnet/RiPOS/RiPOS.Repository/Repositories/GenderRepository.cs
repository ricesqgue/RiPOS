using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories
{
    public class GenderRepository(RiPosDbContext dbContext) : GenericRepository<Gender>(dbContext), IGenderRepository;
}
