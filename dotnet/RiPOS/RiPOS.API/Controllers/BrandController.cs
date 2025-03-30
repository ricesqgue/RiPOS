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
    [Route("api/brands")]
    [Authorize]
    public class BrandController(IBrandService brandService) : ControllerBase
    {
        [HttpGet]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetBrands([FromQuery] bool includeInactives = false)
        {
            var brands = await brandService.GetAllAsync(includeInactives);
            return Ok(brands);
        }

        [HttpGet("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetBrandById([FromRoute] int id)
        {
            var brand = await brandService.GetByIdAsync(id);

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
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> AddBrand([FromBody] BrandRequest request)
        {
            var userId = HttpContext.GetUserId();
            var responseMessage = await brandService.AddAsync(request, userId);

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
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateBrand([FromRoute] int id, [FromBody] BrandRequest request)
        {
            if (!await brandService.ExistsByIdAsync(id))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Marca no encontrada"
                };
                return NotFound(response);
            }
            
            var userId = HttpContext.GetUserId();
            var responseMessage = await brandService.UpdateAsync(id, request, userId);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateBrand([FromRoute] int id)
        {
            if (!await brandService.ExistsByIdAsync(id))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Marca no encontrada"
                };
                return NotFound(response);
            }

            var userId = HttpContext.GetUserId();
            var responseMessage = await brandService.DeactivateAsync(id, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
