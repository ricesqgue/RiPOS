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
    [Route("api/genders")]
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class GenderController(IGenderService genderService) : ControllerBase
    {
        [HttpGet]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(typeof(ICollection<GenderResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<GenderResponse>>> GetGenders([FromQuery] bool includeInactives = false)
        {
            var genders = await genderService.GetAllAsync(includeInactives);
            return Ok(genders);
        }

        [HttpGet("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(typeof(GenderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GenderResponse>> GetGenderById([FromRoute] int id)
        {
            var gender = await genderService.GetByIdAsync(id);

            if (gender == null)
            {
                return NotFound(new SimpleResponse("Género no encontrado"));
            }

            return Ok(gender);
        }

        [HttpPost]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(typeof(MessageResponse<GenderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MessageResponse<GenderResponse>>> AddGender([FromBody] GenderRequest request)
        {
            var userId = HttpContext.GetUserId();
            var responseMessage = await genderService.AddAsync(request, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(new SimpleResponse(responseMessage.Message));
            }

            return Ok(responseMessage);
        }

        [HttpPut("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(typeof(MessageResponse<GenderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MessageResponse<GenderResponse>>> UpdateGender([FromRoute] int id, [FromBody] GenderRequest request)
        {
            if (!await genderService.ExistsByIdAsync(id))
            {
                return NotFound(new SimpleResponse("Género no encontrado"));
            }
            
            var userId = HttpContext.GetUserId();
            var responseMessage = await genderService.UpdateAsync(id, request, userId);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateGender([FromRoute] int id)
        {
            if (!await genderService.ExistsByIdAsync(id))
            {
                return NotFound(new SimpleResponse("Género no encontrado"));
            }

            var userId = HttpContext.GetUserId();
            var responseMessage = await genderService.DeactivateAsync(id, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(new SimpleResponse(responseMessage.Message));
            }

            return Ok(responseMessage.Data);
        }
    }
}
