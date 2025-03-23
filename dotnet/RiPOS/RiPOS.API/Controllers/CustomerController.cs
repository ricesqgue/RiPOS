using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Controllers
{
    [Route("api/customers")]
    public class CustomerController(ICustomerService customerService) : ControllerBase
    {
        private readonly UserSession _session = new UserSession() { CompanyId = 2, UserId = 1 };

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetCustomers([FromQuery] bool includeInactives = false)
        {
            var customers = await customerService.GetAllAsync(_session.CompanyId, includeInactives);
            return Ok(customers);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetCustomerById([FromRoute] int id)
        {
            var customer = await customerService.GetByIdAsync(id, _session.CompanyId);

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
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> AddCustomer([FromBody] CustomerRequest request)
        {
            var responseMessage = await customerService.AddAsync(request, _session);

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
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateCustomer([FromRoute] int id, [FromBody] CustomerRequest request)
        {
            if (!await customerService.ExistsByIdAsync(id, _session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Cliente no encontrado"
                };
                return NotFound(response);
            }

            var responseMessage = await customerService.UpdateAsync(id, request, _session);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateCustomer([FromRoute] int id)
        {
            if (!await customerService.ExistsByIdAsync(id, _session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Cliente no encontrado"
                };
                return NotFound(response);
            }

            var responseMessage = await customerService.DeactivateAsync(id, _session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
