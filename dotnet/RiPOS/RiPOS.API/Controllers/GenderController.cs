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
    public class GenderController(IGenderService genderService) : ControllerBase
    {
        [HttpGet]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetGenders([FromQuery] bool includeInactives = false)
        {
            var genders = await genderService.GetAllAsync(includeInactives);
            return Ok(genders);
        }

        [HttpGet("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetGenderById([FromRoute] int id)
        {
            var gender = await genderService.GetByIdAsync(id);

            if (gender == null)
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Género no encontrado"
                };
                return NotFound(response);
            }

            return Ok(gender);
        }

        [HttpPost]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> AddGender([FromBody] GenderRequest request)
        {
            var userId = HttpContext.GetUserId();
            var responseMessage = await genderService.AddAsync(request, userId);

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
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateGender([FromRoute] int id, [FromBody] GenderRequest request)
        {
            if (!await genderService.ExistsByIdAsync(id))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Género no encontrado"
                };
                return NotFound(response);
            }
            
            var userId = HttpContext.GetUserId();
            var responseMessage = await genderService.UpdateAsync(id, request, userId);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateGender([FromRoute] int id)
        {
            if (!await genderService.ExistsByIdAsync(id))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Género no encontrado"
                };
                return NotFound(response);
            }

            var userId = HttpContext.GetUserId();
            var responseMessage = await genderService.DeactivateAsync(id, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
