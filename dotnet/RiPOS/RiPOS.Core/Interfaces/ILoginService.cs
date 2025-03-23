using System.Security.Claims;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces
{
    public interface ILoginService
    {
        // Task<MessageResponse<UserResponse>> AuthenticateAsync(LoginRequest request);

        // string BuildToken(UserResponse user);
        
        LoginResponse GenerateToken(UserResponse user);
        ClaimsIdentity GetPrincipalFromExpiredToken(string accessToken);
        Task<bool> ValidateRefreshTokenAsync(string refreshToken, int userId, int companyId);
        Task RevokeRefreshTokenAsync(string refreshToken, int userId, int companyId);
    }
}
