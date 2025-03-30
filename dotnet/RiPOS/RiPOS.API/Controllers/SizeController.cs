using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.API.Utilities.Security;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Enums;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;
using RiPOS.Shared.Utilities.Extensions;

namespace RiPOS.API.Controllers
{
    [Route("api/sizes")]
    [Authorize]
    public class SizeController(ISizeService sizeService) : ControllerBase
    {
        [HttpGet]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetSizes([FromQuery] bool includeInactives = false)
        {
            var sizes = await sizeService.GetAllAsync(includeInactives);
            return Ok(sizes);
        }

        [HttpGet("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetSizeById([FromRoute] int id)
        {
            var size = await sizeService.GetByIdAsync(id);

            if (size == null)
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Talla no encontrada"
                };
                return NotFound(response);
            }

            return Ok(size);
        }

        [HttpPost]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> AddSize([FromBody] SizeRequest request)
        {
            var userId = HttpContext.GetUserId();
            var responseMessage = await sizeService.AddAsync(request, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }

        [HttpPut("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateSize([FromRoute] int id, [FromBody] SizeRequest request)
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
            var responseMessage = await sizeService.UpdateAsync(id, request, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }

        [HttpDelete("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<MessageResponse<string>>> DeactivateSize([FromRoute] int id)
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
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
