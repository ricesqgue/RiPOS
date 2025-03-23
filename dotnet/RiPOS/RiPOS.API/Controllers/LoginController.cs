using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Requests;

namespace RiPOS.API.Controllers
{
    [Route("api/login")]
    public class LoginController(ILoginService loginService) : ControllerBase
    {
        [HttpPost]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<string>> GenerateToken([FromBody] LoginRequest request)
        {
            /*var userResponse = await loginService.AuthenticateAsync(request);

            if (!userResponse.Success)
            {
                return Unauthorized(userResponse.Message);
            }

            var token = loginService.BuildToken(userResponse.Data);*/

            return Ok("");
        }
    }
}
