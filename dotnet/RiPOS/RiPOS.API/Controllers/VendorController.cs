using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Controllers
{
    [Route("api/vendors")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;
        private readonly UserSession session = new UserSession() { CompanyId = 2, UserId = 1 };

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetVendors([FromQuery] bool includeInactives = false)
        {
            var vendors = await _vendorService.GetAllAsync(session.CompanyId, includeInactives);
            return Ok(vendors);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetVendorById([FromRoute] int id)
        {
            var vendor = await _vendorService.GetByIdAsync(id, session.CompanyId);

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
            var responseMessage = await _vendorService.AddAsync(request, session);

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
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateVendor([FromRoute] int id, [FromBody] VendorRequest request)
        {
            if (!await _vendorService.ExistsByIdAsync(id, session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Proveedor no encontrado"
                };
                return NotFound(response);
            }

            var responseMessage = await _vendorService.UpdateAsync(id, request, session);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateVendor([FromRoute] int id)
        {
            if (!await _vendorService.ExistsByIdAsync(id, session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Proveedor no encontrado"
                };
                return NotFound(response);
            }

            var responseMessage = await _vendorService.DeactivateAsync(id, session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
