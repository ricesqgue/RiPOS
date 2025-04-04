using RiPOS.Domain.Entities;

namespace RiPOS.Repository.Interfaces;

public interface IAuthRepository
{
    Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);
    Task<bool> AddRefreshTokenAsync(RefreshToken refreshToken);
    Task DeleteUserExpiredTokensAsync(int userId);
}