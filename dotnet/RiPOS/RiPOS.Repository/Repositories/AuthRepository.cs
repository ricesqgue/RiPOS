using Microsoft.EntityFrameworkCore;
using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Repository.Session;

namespace RiPOS.Repository.Repositories;

public class AuthRepository(RiPosDbContext dbContext) : IAuthRepository
{
    public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
    {
        return await dbContext.RefreshTokens.FirstOrDefaultAsync(rf => rf.Token == refreshToken);
    }
    
    public async Task<bool> AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        refreshToken.Created = DateTime.UtcNow;
        await dbContext.RefreshTokens.AddAsync(refreshToken);
        return await dbContext.SaveChangesAsync() > 0;
    }
    
    public async Task DeleteUserExpiredTokensAsync(int userId)
    {
        var expiredTokens = await dbContext.RefreshTokens
            .Where(rf => rf.UserId == userId && rf.Expires < DateTime.UtcNow)
            .ToListAsync();
    
        if (expiredTokens.Any())
        {
            dbContext.RefreshTokens.RemoveRange(expiredTokens);
            await dbContext.SaveChangesAsync();
        }
    }
}