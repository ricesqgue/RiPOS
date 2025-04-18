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
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class VendorController(IVendorService vendorService) : ControllerBase
    {
        [HttpGet]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(typeof(ICollection<VendorResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<VendorResponse>>> GetVendors([FromQuery] bool includeInactives = false)
        {
            var vendors = await vendorService.GetAllAsync(includeInactives);
            return Ok(vendors);
        }

        [HttpGet("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(typeof(VendorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VendorResponse>> GetVendorById([FromRoute] int id)
        {
            var vendor = await vendorService.GetByIdAsync(id);

            if (vendor == null)
            {
                return NotFound(new SimpleResponse("Proveedor no encontrado"));
            }

            return Ok(vendor);
        }

        [HttpPost]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(typeof(VendorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VendorResponse>> AddVendor([FromBody] VendorRequest request)
        {
            var userId = HttpContext.GetUserId();
            var responseMessage = await vendorService.AddAsync(request, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(new SimpleResponse(responseMessage.Message));
            }

            return Ok(responseMessage.Data);
        }

        [HttpPut("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(typeof(VendorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VendorResponse>> UpdateVendor([FromRoute] int id, [FromBody] VendorRequest request)
        {
            if (!await vendorService.ExistsByIdAsync(id))
            {
                return NotFound(new SimpleResponse("Proveedor no encontrado"));
            }

            var userId = HttpContext.GetUserId();
            var responseMessage = await vendorService.UpdateAsync(id, request, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(new SimpleResponse(responseMessage.Message));
            }

            return Ok(responseMessage.Data);
        }

        [HttpDelete("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse>> DeactivateVendor([FromRoute] int id)
        {
            if (!await vendorService.ExistsByIdAsync(id))
            {
                return NotFound(new SimpleResponse("Proveedor no encontrado"));
            }

            var userId = HttpContext.GetUserId();
            var responseMessage = await vendorService.DeactivateAsync(id, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(new SimpleResponse(responseMessage.Message));
            }

            return Ok(responseMessage.Data);
        }
    }
}
