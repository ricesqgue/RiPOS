using Microsoft.AspNetCore.Mvc;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserSession session = new UserSession() { CompanyId = 2, UserId = 1, StoreId = 11 };

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("company/users")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<UserResponse>>> GetCompanyUsers([FromQuery] bool includeInactives = false)
        {
            var users = await _userService.GetAllByCompanyAsync(session.CompanyId, includeInactives);
            return Ok(users);
        }

        [HttpGet("company/users/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ICollection<UserResponse>>> GetUserByIdInCompany([FromRoute] int id)
        {
            var user = await _userService.GetByIdInCompanyAsync(id, session.CompanyId);

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
            var users = await _userService.GetAllByStoreAsync(session.StoreId, includeInactives);
            return Ok(users);
        }

        [HttpGet("store/users/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ICollection<UserResponse>>> GetUserByIdInStore([FromRoute] int id)
        {
            var user = await _userService.GetByIdInStoreAsync(id, session.StoreId);

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
