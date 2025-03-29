using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories
{
    public class ColorRepository(RiPosDbContext dbContext) : GenericRepository<Color>(dbContext), IColorRepository;
}
