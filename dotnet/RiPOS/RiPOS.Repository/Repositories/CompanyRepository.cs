using Microsoft.EntityFrameworkCore;
using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private RiPOSDbContext _dbContext;
        public CompanyRepository(RiPOSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            return await _dbContext.Companies.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
        }
    }
}
