using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Controllers
{
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly UserSession session = new UserSession() { CompanyId = 2, UserId = 1 };

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetCustomers([FromQuery] bool includeInactives = false)
        {
            var customers = await _customerService.GetAllAsync(session.CompanyId, includeInactives);
            return Ok(customers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetCustomerById([FromRoute] int id)
        {
            var customer = await _customerService.GetByIdAsync(id, session.CompanyId);

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
            var responseMessage = await _customerService.AddAsync(request, session);

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
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateCustomer([FromRoute] int id, [FromBody] CustomerRequest request)
        {
            if (!await _customerService.ExistsByIdAsync(id, session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Cliente no encontrado"
                };
                return NotFound(response);
            }

            var responseMessage = await _customerService.UpdateAsync(id, request, session);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateCustomer([FromRoute] int id)
        {
            if (!await _customerService.ExistsByIdAsync(id, session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Cliente no encontrado"
                };
                return NotFound(response);
            }

            var responseMessage = await _customerService.DeactivateAsync(id, session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
