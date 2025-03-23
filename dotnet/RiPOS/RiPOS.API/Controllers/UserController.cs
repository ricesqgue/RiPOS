using Microsoft.AspNetCore.Mvc;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly UserSession _session = new UserSession() { CompanyId = 2, UserId = 1, StoreId = 11 };

        [HttpGet("company/users")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<UserResponse>>> GetCompanyUsers([FromQuery] bool includeInactives = false)
        {
            var users = await userService.GetAllByCompanyAsync(_session.CompanyId, includeInactives);
            return Ok(users);
        }

        [HttpGet("company/users/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ICollection<UserResponse>>> GetUserByIdInCompany([FromRoute] int id)
        {
            var user = await userService.GetByIdInCompanyAsync(id, _session.CompanyId);

            if (user == null)
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Usuario no encontrado"
                };
                return NotFound(response);
            }

            return Ok(user);
        }

        [HttpGet("store/users")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<UserResponse>>> GetStoreUsers([FromQuery] bool includeInactives = false)
        {
            var users = await userService.GetAllByStoreAsync(_session.StoreId, includeInactives);
            return Ok(users);
        }

        [HttpGet("store/users/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ICollection<UserResponse>>> GetUserByIdInStore([FromRoute] int id)
        {
            var user = await userService.GetByIdInStoreAsync(id, _session.StoreId);

            if (user == null)
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Usuario no encontrado"
                };
                return NotFound(response);
            }

            return Ok(user);
        }
    }
}
