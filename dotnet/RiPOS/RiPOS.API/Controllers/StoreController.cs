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
    [Route("api/stores")]
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class StoreController(IStoreService storeService) : ControllerBase
    {
        [HttpGet]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(typeof(ICollection<StoreResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetStores()
        {
            var stores = await storeService.GetAllAsync();
            return Ok(stores);
        }

        [HttpGet("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(typeof(StoreResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StoreResponse>> GetStoreById([FromRoute] int id)
        {
            var store = await storeService.GetByIdAsync(id);

            if (store == null)
            {
                return NotFound(new SimpleResponse("Tienda no encontrada"));
            }

            return Ok(store);
        }

        [HttpPost]
        [RoleAuthorize([RoleEnum.SuperAdmin])]
        [ModelValidation]
        [ProducesResponseType(typeof(MessageResponse<StoreResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> AddStore([FromBody] StoreRequest request)
        {
            var userId = HttpContext.GetUserId();
            var responseMessage = await storeService.AddAsync(request, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(new SimpleResponse(responseMessage.Message));
            }

            return Ok(responseMessage);
        }

        [HttpPut("{id:int}")]
        [RoleAuthorize([RoleEnum.SuperAdmin])]
        [ModelValidation]
        [ProducesResponseType(typeof(MessageResponse<StoreResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateStore([FromRoute] int id, [FromBody] StoreRequest request)
        {
            if (!await storeService.ExistsByIdAsync(id))
            {
                return NotFound(new SimpleResponse("Tienda no encontrada"));
            }

            var userId = HttpContext.GetUserId();
            var responseMessage = await storeService.UpdateAsync(id, request, userId);

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
        public async Task<ActionResult<SimpleResponse>> DeactivateStore([FromRoute] int id)
        {
            if (!await storeService.ExistsByIdAsync(id))
            {
                return NotFound(new SimpleResponse("Tienda no encontrada"));
            }

            var userId = HttpContext.GetUserId();
            var responseMessage = await storeService.DeactivateAsync(id, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(new SimpleResponse(responseMessage.Message));
            }

            return Ok(responseMessage.Data);
        }
    }
}
