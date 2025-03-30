using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Controllers
{
    [Route("api/login")]
    public class LoginController(ILoginService loginService) : ControllerBase
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
                var tokenResponse = loginService.BuildTokens(userResponse.Data);
                return Ok(tokenResponse);
            }
            
            return Unauthorized(userResponse.Message);
        }
    }
}
