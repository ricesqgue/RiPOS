using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Controllers
{
    [Route("api/vendors")]
    public class VendorController(IVendorService vendorService) : ControllerBase
    {
        private readonly UserSession _session = new UserSession() { CompanyId = 2, UserId = 1 };

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetVendors([FromQuery] bool includeInactives = false)
        {
            var vendors = await vendorService.GetAllAsync(_session.CompanyId, includeInactives);
            return Ok(vendors);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetVendorById([FromRoute] int id)
        {
            var vendor = await vendorService.GetByIdAsync(id, _session.CompanyId);

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
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> AddVendor([FromBody] VendorRequest request)
        {
            var responseMessage = await vendorService.AddAsync(request, _session);

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
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateVendor([FromRoute] int id, [FromBody] VendorRequest request)
        {
            if (!await vendorService.ExistsByIdAsync(id, _session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Proveedor no encontrado"
                };
                return NotFound(response);
            }

            var responseMessage = await vendorService.UpdateAsync(id, request, _session);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateVendor([FromRoute] int id)
        {
            if (!await vendorService.ExistsByIdAsync(id, _session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Proveedor no encontrado"
                };
                return NotFound(response);
            }

            var responseMessage = await vendorService.DeactivateAsync(id, _session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
