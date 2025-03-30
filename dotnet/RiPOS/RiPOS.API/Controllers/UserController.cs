using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.Security;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Enums;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Utilities.Extensions;

namespace RiPOS.API.Controllers
{
    [Route("api")]
    [Authorize]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet("users")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ICollection<UserResponse>>> GetUsers([FromQuery] bool includeInactives = false)
        {
            var users = await userService.GetAllAsync(includeInactives);
            return Ok(users);
        }
        
        [HttpGet("store/users")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ICollection<UserResponse>>> GetStoreUsers([FromQuery] bool includeInactives = false)
        {
            var storeId = HttpContext.GetHeaderStoreId();
            var users = await userService.GetAllByStoreAsync(storeId, includeInactives);
            return Ok(users);
        }

        [HttpGet("store/users/{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ICollection<UserResponse>>> GetUserByIdInStore([FromRoute] int id)
        {
            var storeId = HttpContext.GetHeaderStoreId();
            var user = await userService.GetByIdInStoreAsync(id, storeId);

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
