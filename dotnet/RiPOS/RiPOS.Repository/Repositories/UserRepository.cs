using Microsoft.EntityFrameworkCore;
using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Enums;

namespace RiPOS.Repository.Repositories;

public class UserRepository(RiPosDbContext dbContext) : GenericRepository<User>(dbContext), IUserRepository
{
    private readonly RiPosDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Role>> GetStoreRolesAsync(int userId, int storeId)
    {
        var userRoles = await _dbContext.UserStoreRoles
            .AsNoTracking()
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == userId && ur.StoreId == storeId)
            .Select(ur => ur.Role)
            .ToListAsync();
            
        return userRoles;
    }
}