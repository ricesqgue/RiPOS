using Microsoft.EntityFrameworkCore;
using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories
{
    public class MiscellaneousRepository : IMiscellaneousRepository
    {
        private readonly RiPosDbContext _dbContext;

        public MiscellaneousRepository(RiPosDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<CountryState>> GetAllCountryStatesAsync()
        {
            return await _dbContext.CountryStates.AsNoTracking().ToListAsync();
        }
    }
}
