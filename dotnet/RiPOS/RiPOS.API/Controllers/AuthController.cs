using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Controllers
{
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] AuthRequest request)
        {
            var userResponse = await authService.AuthenticateAsync(request);

            if (userResponse is { Success: true, Data: not null })
            {
                var tokenResponse = await authService.BuildAndStoreTokensAsync(userResponse.Data);
                SetRefreshTokenCookie(tokenResponse.RefreshToken);
                return Ok(new LoginResponse
                {
                    AccessToken = tokenResponse.AccessToken,
                    Expires = tokenResponse.Expires,
                    AvailableStores = tokenResponse.AvailableStores
                });
            }
            return Unauthorized(userResponse.Message);
        }

        [HttpPost("refreshToken")]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Refresh token no encontrado");
            }

            var tokenResponse = await authService.RefreshTokenAsync(request.AccessToken, refreshToken);

            if (tokenResponse is { Success: true, Data: not null })
            {
                SetRefreshTokenCookie(tokenResponse.Data.RefreshToken);
                return Ok(new LoginResponse()
                {
                    AccessToken = tokenResponse.Data.AccessToken,
                    Expires = tokenResponse.Data.Expires,
                    AvailableStores = tokenResponse.Data.AvailableStores
                });
            }

            return Unauthorized(tokenResponse.Message);
        }

        [HttpPost("logout")]
        [ProducesResponseType(200)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("refreshToken");
            return Ok();
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(24)
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
