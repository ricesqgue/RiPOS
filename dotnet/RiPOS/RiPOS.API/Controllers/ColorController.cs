using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.API.Controllers
{
    [Route("api/colors")]
    public class ColorController(IColorService colorService) : ControllerBase
    {
        private readonly UserSession _session = new UserSession() { CompanyId = 2, UserId = 1 };

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetColors([FromQuery] bool includeInactives = false)
        {
            var colors = await colorService.GetAllAsync(_session.CompanyId, includeInactives);
            return Ok(colors);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetColorById([FromRoute] int id)
        {
            var color = await colorService.GetByIdAsync(id, _session.CompanyId);

            if (color == null)
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Color no encontrado"
                };
                return NotFound(response);
            }

            return Ok(color);
        }

        [HttpPost]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> AddColor([FromBody] ColorRequest request)
        {
            var responseMessage = await colorService.AddAsync(request, _session);

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
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateColor([FromRoute] int id, [FromBody] ColorRequest request)
        {
            if (!await colorService.ExistsByIdAsync(id, _session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Color no encontrado"
                };
                return NotFound(response);
            }

            var responseMessage = await colorService.UpdateAsync(id, request, _session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }

        [HttpDelete("{id:int}")]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<MessageResponse<string>>> DeactivateColor([FromRoute] int id)
        {
            if (!await colorService.ExistsByIdAsync(id, _session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Color no encontrado"
                };
                return NotFound(response);
            }

            var responseMessage = await colorService.DeactivateAsync(id, _session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
