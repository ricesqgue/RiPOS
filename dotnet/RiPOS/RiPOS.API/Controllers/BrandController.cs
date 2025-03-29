using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.API.Utilities.Security;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Enums;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.API.Controllers
{
    [Route("api/brands")]
    [RoleAuthorize([RoleEnum.Admin])]
    public class BrandController(IBrandService brandService) : ControllerBase
    {
        private readonly UserSession _session = new UserSession() { CompanyId = 2, UserId = 1 };

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetBrands([FromQuery] bool includeInactives = false)
        {
            var user = HttpContext.User;
            var brands = await brandService.GetAllAsync(_session.CompanyId, includeInactives);
            return Ok(brands);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetBrandById([FromRoute] int id)
        {
            var brand = await brandService.GetByIdAsync(id, _session.CompanyId);

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
            var responseMessage = await brandService.AddAsync(request, _session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }

        [HttpPut("{id:int}")]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateBrand([FromRoute] int id, [FromBody] BrandRequest request)
        {
            if (!await brandService.ExistsByIdAsync(id, _session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Marca no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await brandService.UpdateAsync(id, request, _session);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateBrand([FromRoute] int id)
        {
            if (!await brandService.ExistsByIdAsync(id, _session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Marca no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await brandService.DeactivateAsync(id, _session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
