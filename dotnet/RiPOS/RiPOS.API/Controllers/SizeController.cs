using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.API.Utilities.Security;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Enums;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Utilities.Extensions;

namespace RiPOS.API.Controllers
{
    [Route("api/sizes")]
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class SizeController(ISizeService sizeService) : ControllerBase
    {
        [HttpGet]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(typeof(ICollection<SizeResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<SizeResponse>>> GetSizes([FromQuery] bool includeInactives = false)
        {
            var sizes = await sizeService.GetAllAsync(includeInactives);
            return Ok(sizes);
        }

        [HttpGet("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(typeof(SizeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SizeResponse>> GetSizeById([FromRoute] int id)
        {
            var size = await sizeService.GetByIdAsync(id);

            if (size == null)
            {
                return NotFound(new SimpleResponse("Talla no encontrada"));
            }

            return Ok(size);
        }

        [HttpPost]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(typeof(SizeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SizeResponse>> AddSize([FromBody] SizeRequest request)
        {
            var userId = HttpContext.GetUserId();
            var responseMessage = await sizeService.AddAsync(request, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(new SimpleResponse(responseMessage.Message));
            }

            return Ok(responseMessage.Data);
        }

        [HttpPut("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(typeof(SizeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SizeResponse>> UpdateSize([FromRoute] int id, [FromBody] SizeRequest request)
        {
            if (!await sizeService.ExistsByIdAsync(id))
            {
                return NotFound(new SimpleResponse("Talla no encontrada"));
            }
            
            var userId = HttpContext.GetUserId();
            var responseMessage = await sizeService.UpdateAsync(id, request, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(new SimpleResponse(responseMessage.Message));
            }

            return Ok(responseMessage.Data);
        }

        [HttpDelete("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse>> DeactivateSize([FromRoute] int id)
        {
            if (!await sizeService.ExistsByIdAsync(id))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Talla no encontrada"
                };
                return NotFound(response);
            }
            
            var userId = HttpContext.GetUserId();
            var responseMessage = await sizeService.DeactivateAsync(id, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(new SimpleResponse(responseMessage.Message));
            }

            return Ok(responseMessage.Data);
        }
    }
}
