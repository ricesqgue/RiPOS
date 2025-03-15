using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.API.Controllers
{
    [Route("api/brands")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly UserSession session = new UserSession() { CompanyId = 2, UserId = 1 };

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetBrands([FromQuery] bool includeInactives = false)
        {
            var brands = await _brandService.GetAllAsync(session.CompanyId, includeInactives);
            return Ok(brands);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetBrandById([FromRoute] int id)
        {
            var brand = await _brandService.GetByIdAsync(id, session.CompanyId);

            if (brand == null)
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Marca no encontrada"
                };
                return NotFound(response);
            }

            return Ok(brand);
        }

        [HttpPost]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> AddBrand([FromBody] BrandRequest request)
        {
            var responseMessage = await _brandService.AddAsync(request, session);

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
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateBrand([FromRoute] int id, [FromBody] BrandRequest request)
        {
            if (!await _brandService.ExistsByIdAsync(id, session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Marca no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await _brandService.UpdateAsync(id, request, session);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateBrand([FromRoute] int id)
        {
            if (!await _brandService.ExistsByIdAsync(id, session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Marca no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await _brandService.DeactivateAsync(id, session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
