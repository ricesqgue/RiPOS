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
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class BrandController(IBrandService brandService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<BrandResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<BrandResponse>>> GetBrands([FromQuery] bool includeInactives = false)
        {
            var brands = await brandService.GetAllAsync(includeInactives);
            return Ok(brands);
        }

        [HttpGet("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(typeof(BrandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BrandResponse>> GetBrandById([FromRoute] int id)
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
        [ProducesResponseType(typeof(MessageResponse<BrandResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MessageResponse<BrandResponse>>> AddBrand([FromBody] BrandRequest request)
        {
            var userId = HttpContext.GetUserId();
            var responseMessage = await brandService.AddAsync(request, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(new SimpleResponse(responseMessage.Message));
            }

            return Ok(responseMessage);
        }

        [HttpPut("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(typeof(MessageResponse<BrandResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MessageResponse<BrandResponse>>> UpdateBrand([FromRoute] int id, [FromBody] BrandRequest request)
        {
            if (!await brandService.ExistsByIdAsync(id))
            {
                var response = new SimpleResponse("Marca no encontrada");
                return NotFound(response);
            }
            
            var userId = HttpContext.GetUserId();
            var responseMessage = await brandService.UpdateAsync(id, request, userId);

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
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse>> DeactivateBrand([FromRoute] int id)
        {
            if (!await brandService.ExistsByIdAsync(id))
            {
                return NotFound(new SimpleResponse("Marca no encontrada"));
            }

            var userId = HttpContext.GetUserId();
            var responseMessage = await brandService.DeactivateAsync(id, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(new SimpleResponse(responseMessage.Message));
            }

            return Ok(responseMessage.Data);
        }
    }
}
