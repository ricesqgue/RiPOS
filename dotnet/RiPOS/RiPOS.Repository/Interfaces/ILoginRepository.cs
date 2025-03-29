using RiPOS.Domain.Entities;

namespace RiPOS.Repository.Interfaces;

public interface ILoginRepository
{
    Task<RefreshToken> GetRefreshTokenAsync(string refreshToken);
    Task<bool> AddRefreshTokenAsync(RefreshToken refreshToken);
    Task<bool> UpdateRefreshTokenAsync(int id, string token, DateTime expires);
    Task<bool> DeleteRefreshTokenAsync(RefreshToken refreshToken);
}