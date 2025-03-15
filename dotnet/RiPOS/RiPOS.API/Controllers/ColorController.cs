using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.API.Controllers
{
    [Route("api/colors")]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _colorService;
        private readonly UserSession session = new UserSession() { CompanyId = 2, UserId = 1 };

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetColors([FromQuery] bool includeInactives = false)
        {
            var colors = await _colorService.GetAllAsync(session.CompanyId, includeInactives);
            return Ok(colors);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetColorById([FromRoute] int id)
        {
            var color = await _colorService.GetByIdAsync(id, session.CompanyId);

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
            var responseMessage = await _colorService.AddAsync(request, session);

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
            if (!await _colorService.ExistsByIdAsync(id, session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Color no encontrado"
                };
                return NotFound(response);
            }

            var responseMessage = await _colorService.UpdateAsync(id, request, session);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateColor([FromRoute] int id)
        {
            if (!await _colorService.ExistsByIdAsync(id, session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Color no encontrado"
                };
                return NotFound(response);
            }

            var responseMessage = await _colorService.DeactivateAsync(id, session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
