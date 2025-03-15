using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.API.Controllers
{
    [Route("api/genders")]
    public class GenderController : ControllerBase
    {
        private readonly IGenderService _genderService;
        private readonly UserSession session = new UserSession() { CompanyId = 2, UserId = 1 };

        public GenderController(IGenderService genderService)
        {
            _genderService = genderService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetGenders([FromQuery] bool includeInactives = false)
        {
            var genders = await _genderService.GetAllAsync(session.CompanyId, includeInactives);
            return Ok(genders);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetGenderById([FromRoute] int id)
        {
            var gender = await _genderService.GetByIdAsync(id, session.CompanyId);

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
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> AddGender([FromBody] GenderRequest request)
        {
            var responseMessage = await _genderService.AddAsync(request, session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }

        [HttpPut("{id}")]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateGender([FromRoute] int id, [FromBody] GenderRequest request)
        {
            if (!await _genderService.ExistsByIdAsync(id, session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Género no encontrado"
                };
                return NotFound(response);
            }

            var responseMessage = await _genderService.UpdateAsync(id, request, session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }

        [HttpDelete("{id}")]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<MessageResponse<string>>> DeactivateGender([FromRoute] int id)
        {
            if (!await _genderService.ExistsByIdAsync(id, session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Género no encontrado"
                };
                return NotFound(response);
            }

            var responseMessage = await _genderService.DeactivateAsync(id, session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
