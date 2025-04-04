using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Controllers
{
    [Route("api/login")]
    public class AuthController(ILoginService loginService) : ControllerBase
    {
        [HttpPost]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
        {
            var userResponse = await loginService.AuthenticateAsync(request);
            
            if (userResponse is { Success: true, Data: not null })
            {
                var tokenResponse = await loginService.BuildAndStoreTokensAsync(userResponse.Data);
                return Ok(tokenResponse);
            }
            
            return Unauthorized(userResponse.Message);
        }

        [HttpPost("refreshToken")]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<TokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Refresh token no encontrado");
            }
            
            var tokenResponse = await loginService.RefreshTokenAsync(request.AccessToken, refreshToken);

            if (tokenResponse is { Success: true, Data: not null })
            {
                return Ok(tokenResponse.Data);
            }

            return Unauthorized(tokenResponse.Message);
        }
    }
}
