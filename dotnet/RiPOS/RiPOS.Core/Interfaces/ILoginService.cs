using System.Security.Claims;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces
{
    public interface ILoginService
    {
        Task<MessageResponse<UserResponse>> AuthenticateAsync(LoginRequest request);

        TokenResponse BuildTokens(UserResponse user);
    }
}
