using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories
{
    public class CategoryRepository(RiPosDbContext dbContext)
        : GenericRepository<Category>(dbContext), ICategoryRepository;
}
