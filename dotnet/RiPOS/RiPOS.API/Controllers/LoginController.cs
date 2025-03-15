using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Requests;

namespace RiPOS.API.Controllers
{
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<string>> GenerateToken([FromBody] LoginRequest request)
        {
            var userResponse = await _loginService.AuthenticateAsync(request);

            if (!userResponse.Success)
            {
                return Unauthorized(userResponse.Message);
            }

            var token = _loginService.BuildToken(userResponse.Data);

            return Ok("");
        }
    }
}
