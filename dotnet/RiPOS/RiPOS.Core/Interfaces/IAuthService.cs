using System.Security.Claims;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces
{
    public interface IAuthService
    {
        Task<MessageResponse<UserResponse>> AuthenticateAsync(AuthRequest request);

        Task<TokenResponse> BuildAndStoreTokensAsync(UserResponse user);
        
        Task<MessageResponse<TokenResponse>> RefreshTokenAsync(string accessToken, string refreshToken);
    }
}
