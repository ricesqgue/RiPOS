using Microsoft.EntityFrameworkCore;
using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories;

public class LoginRepository(RiPosDbContext dbContext) : ILoginRepository
{
    public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
    {
        return await dbContext.RefreshTokens.FirstOrDefaultAsync(rf => rf.Token == refreshToken);
    }

    public async Task<bool> AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        refreshToken.Created = DateTime.Now;
        await dbContext.RefreshTokens.AddAsync(refreshToken);
        return await dbContext.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> DeleteRefreshTokenAsync(RefreshToken refreshToken)
    {
        dbContext.RefreshTokens.Remove(refreshToken);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateRefreshTokenAsync(int id, string token, DateTime expires)
    {
        var refreshToken = await dbContext.RefreshTokens.FindAsync(id);
        if (refreshToken == null) return false;
        
        refreshToken.Expires = expires;
        refreshToken.Token = token;
        refreshToken.Created = DateTime.Now;
        dbContext.RefreshTokens.Update(refreshToken);
        return await dbContext.SaveChangesAsync() > 0;
    }
}