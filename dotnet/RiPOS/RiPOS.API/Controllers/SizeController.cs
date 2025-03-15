using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.API.Controllers
{
    [Route("api/sizes")]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _sizeService;
        private readonly UserSession session = new UserSession() { CompanyId = 2, UserId = 1 };

        public SizeController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetSizes([FromQuery] bool includeInactives = false)
        {
            var sizes = await _sizeService.GetAllAsync(session.CompanyId, includeInactives);
            return Ok(sizes);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetSizeById([FromRoute] int id)
        {
            var size = await _sizeService.GetByIdAsync(id, session.CompanyId);

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
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> AddSize([FromBody] SizeRequest request)
        {
            var responseMessage = await _sizeService.AddAsync(request, session);

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
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateSize([FromRoute] int id, [FromBody] SizeRequest request)
        {
            if (!await _sizeService.ExistsByIdAsync(id, session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Talla no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await _sizeService.UpdateAsync(id, request, session);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateSize([FromRoute] int id)
        {
            if (!await _sizeService.ExistsByIdAsync(id, session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Talla no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await _sizeService.DeactivateAsync(id, session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
