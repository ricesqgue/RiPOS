using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.Security;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Enums;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Utilities.Extensions;

namespace RiPOS.API.Controllers;

[Route("api")]
[Authorize]
[Consumes("application/json")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("users")]
    [RoleAuthorize([RoleEnum.Admin])]
    [ProducesResponseType(typeof(ICollection<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<UserResponse>>> GetUsers([FromQuery] bool includeInactives = false)
    {
        var users = await userService.GetAllAsync(includeInactives);
        return Ok(users);
    }
        
    [HttpGet("store/users")]
    [RoleAuthorize([RoleEnum.Admin])]
    [ProducesResponseType(typeof(ICollection<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<UserResponse>>> GetStoreUsers([FromQuery] bool includeInactives = false)
    {
        var storeId = HttpContext.GetHeaderStoreId();
        var users = await userService.GetAllByStoreAsync(storeId, includeInactives);
        return Ok(users);
    }

    [HttpGet("store/users/{id:int}")]
    [RoleAuthorize([RoleEnum.Admin])]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> GetUserByIdInStore([FromRoute] int id)
    {
        var storeId = HttpContext.GetHeaderStoreId();
        var user = await userService.GetByIdInStoreAsync(id, storeId);

        if (user == null)
        {
            return NotFound(new SimpleResponse("Usuario no encontrado"));
        }

        return Ok(user);
    }
}