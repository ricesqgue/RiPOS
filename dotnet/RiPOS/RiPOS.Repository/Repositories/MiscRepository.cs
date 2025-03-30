// using Microsoft.EntityFrameworkCore;
// using RiPOS.Database;
// using RiPOS.Domain.Entities;
// using RiPOS.Repository.Interfaces;
//
// namespace RiPOS.Repository.Repositories
// {
//     public class MiscRepository(RiPosDbContext dbContext) : IMiscRepository
//     {
//         public async Task<ICollection<CountryState>> GetAllCountryStatesAsync()
//         {
//             return await dbContext.CountryStates.AsNoTracking().ToListAsync();
//         }
//     }
// }
