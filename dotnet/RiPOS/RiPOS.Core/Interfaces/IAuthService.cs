using System.Security.Claims;
using RiPOS.Repository.Repositories;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces
{
    public interface IAuthService
    {
        Task<MessageResponse<UserWithStoresResponse>> AuthenticateAsync(AuthRequest request);

        Task<TokenResponse> BuildAndStoreTokensAsync(UserWithStoresResponse user);
        
        Task<MessageResponse<TokenResponse>> RefreshTokenAsync(string accessToken, string refreshToken);
    }
}
