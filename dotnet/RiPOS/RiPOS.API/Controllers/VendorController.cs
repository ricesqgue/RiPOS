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
    [Route("api/vendors")]
    [Authorize]
    public class VendorController(IVendorService vendorService) : ControllerBase
    {
        [HttpGet]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ICollection<VendorResponse>>> GetVendors([FromQuery] bool includeInactives = false)
        {
            var vendors = await vendorService.GetAllAsync(includeInactives);
            return Ok(vendors);
        }

        [HttpGet("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<VendorResponse>> GetVendorById([FromRoute] int id)
        {
            var vendor = await vendorService.GetByIdAsync(id);

            if (vendor == null)
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Proveedor no encontrado"
                };
                return NotFound(response);
            }

            return Ok(vendor);
        }

        [HttpPost]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MessageResponse<VendorResponse>>> AddVendor([FromBody] VendorRequest request)
        {
            var userId = HttpContext.GetUserId();
            var responseMessage = await vendorService.AddAsync(request, userId);

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
        public async Task<ActionResult<MessageResponse<VendorResponse>>> UpdateVendor([FromRoute] int id, [FromBody] VendorRequest request)
        {
            if (!await vendorService.ExistsByIdAsync(id))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Proveedor no encontrado"
                };
                return NotFound(response);
            }

            var userId = HttpContext.GetUserId();
            var responseMessage = await vendorService.UpdateAsync(id, request, userId);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateVendor([FromRoute] int id)
        {
            if (!await vendorService.ExistsByIdAsync(id))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Proveedor no encontrado"
                };
                return NotFound(response);
            }

            var userId = HttpContext.GetUserId();
            var responseMessage = await vendorService.DeactivateAsync(id, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
