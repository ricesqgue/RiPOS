using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.API.Utilities.Security;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Enums;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Utilities.Extensions;

namespace RiPOS.API.Controllers;

[Route("api/colors")]
[Authorize]
[Consumes("application/json")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class ColorController(IColorService colorService) : ControllerBase
{
    [HttpGet]
    [RoleAuthorize([RoleEnum.Admin])]
    [ProducesResponseType(typeof(ICollection<ColorResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<ColorResponse>>> GetColors([FromQuery] bool includeInactives = false)
    {
        var colors = await colorService.GetAllAsync(includeInactives);
        return Ok(colors);
    }

    [HttpGet("{id:int}")]
    [RoleAuthorize([RoleEnum.Admin])]
    [ProducesResponseType(typeof(ColorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ColorResponse>> GetColorById([FromRoute] int id)
    {
        var color = await colorService.GetByIdAsync(id);

        if (color == null)
        {
            return NotFound(new SimpleResponse("Color no encontrado"));
        }

        return Ok(color);
    }

    [HttpPost]
    [RoleAuthorize([RoleEnum.Admin])]
    [ModelValidation]
    [ProducesResponseType(typeof(MessageResponse<ColorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MessageResponse<ColorResponse>>> AddColor([FromBody] ColorRequest request)
    {
        var userId = HttpContext.GetUserId();
        var responseMessage = await colorService.AddAsync(request, userId);

        if (!responseMessage.Success)
        {
            return BadRequest(new SimpleResponse(responseMessage.Message));
        }

        return Ok(responseMessage);
    }

    [HttpPut("{id}")]
    [RoleAuthorize([RoleEnum.Admin])]
    [ModelValidation]
    [ProducesResponseType(typeof(MessageResponse<ColorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse<ColorResponse>>> UpdateColor([FromRoute] int id, [FromBody] ColorRequest request)
    {
        if (!await colorService.ExistsByIdAsync(id))
        {
            return NotFound(new SimpleResponse("Color no encontrado"));
        }
        var userId = HttpContext.GetUserId();
        var responseMessage = await colorService.UpdateAsync(id, request, userId);

        if (!responseMessage.Success)
        {
            return BadRequest(new SimpleResponse(responseMessage.Message));
        }

        return Ok(responseMessage);
    }

    [HttpDelete("{id:int}")]
    [RoleAuthorize([RoleEnum.Admin])]
    [ModelValidation]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SimpleResponse>> DeactivateColor([FromRoute] int id)
    {
        if (!await colorService.ExistsByIdAsync(id))
        {
            return NotFound(new SimpleResponse("Color no encontrado"));
        }

        var userId = HttpContext.GetUserId();
        var responseMessage = await colorService.DeactivateAsync(id, userId);

        if (!responseMessage.Success)
        {
            return BadRequest(new SimpleResponse(responseMessage.Message));
        }

        return Ok(responseMessage.Data);
    }
}