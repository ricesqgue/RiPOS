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
    [Route("api/customers")]
    [Authorize]
    public class CustomerController(ICustomerService customerService) : ControllerBase
    {
        [HttpGet]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ICollection<CustomerResponse>>> GetCustomers([FromQuery] bool includeInactives = false)
        {
            var customers = await customerService.GetAllAsync(includeInactives);
            return Ok(customers);
        }

        [HttpGet("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CustomerResponse>> GetCustomerById([FromRoute] int id)
        {
            var customer = await customerService.GetByIdAsync(id);

            if (customer == null)
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Cliente no encontrado"
                };
                return NotFound(response);
            }

            return Ok(customer);
        }

        [HttpPost]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<MessageResponse<CustomerResponse>>> AddCustomer([FromBody] CustomerRequest request)
        {
            var userId = HttpContext.GetUserId();
            var responseMessage = await customerService.AddAsync(request, userId);

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
        public async Task<ActionResult<MessageResponse<CustomerResponse>>> UpdateCustomer([FromRoute] int id, [FromBody] CustomerRequest request)
        {
            if (!await customerService.ExistsByIdAsync(id))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Cliente no encontrado"
                };
                return NotFound(response);
            }
            var userId = HttpContext.GetUserId();
            var responseMessage = await customerService.UpdateAsync(id, request, userId);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateCustomer([FromRoute] int id)
        {
            if (!await customerService.ExistsByIdAsync(id))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Cliente no encontrado"
                };
                return NotFound(response);
            }

            var userId = HttpContext.GetUserId();
            var responseMessage = await customerService.DeactivateAsync(id, userId);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
