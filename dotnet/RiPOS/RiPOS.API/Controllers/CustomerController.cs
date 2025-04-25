using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.API.Utilities.Security;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Enums;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Utilities.Extensions;

namespace RiPOS.API.Controllers;

[Route("api/customers")]
[Authorize]
[Consumes("application/json")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class CustomerController(ICustomerService customerService) : ControllerBase
{
    [HttpGet]
    [RoleAuthorize([RoleEnum.Admin])]
    [ProducesResponseType(typeof(ICollection<CustomerResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<CustomerResponse>>> GetCustomers([FromQuery] bool includeInactives = false)
    {
        var customers = await customerService.GetAllAsync(includeInactives);
        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    [RoleAuthorize([RoleEnum.Admin])]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerResponse>> GetCustomerById([FromRoute] int id)
    {
        var customer = await customerService.GetByIdAsync(id);

        if (customer == null)
        {
            return NotFound(new SimpleResponse("Cliente no encontrado"));
        }

        return Ok(customer);
    }

    [HttpPost]
    [RoleAuthorize([RoleEnum.Admin])]
    [ModelValidation]
    [ProducesResponseType(typeof(MessageResponse<CustomerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MessageResponse<CustomerResponse>>> AddCustomer([FromBody] CustomerRequest request)
    {
        var userId = HttpContext.GetUserId();
        var responseMessage = await customerService.AddAsync(request, userId);

        if (!responseMessage.Success)
        {
            return BadRequest(new SimpleResponse(responseMessage.Message));
        }

        return Ok(responseMessage);
    }

    [HttpPut("{id:int}")]
    [RoleAuthorize([RoleEnum.Admin])]
    [ModelValidation]
    [ProducesResponseType(typeof(MessageResponse<CustomerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse<MessageResponse<CustomerResponse>>>> UpdateCustomer([FromRoute] int id, [FromBody] CustomerRequest request)
    {
        if (!await customerService.ExistsByIdAsync(id))
        {
            return NotFound(new SimpleResponse("Cliente no encontrado"));
        }
        var userId = HttpContext.GetUserId();
        var responseMessage = await customerService.UpdateAsync(id, request, userId);

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
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ActionResult<SimpleResponse>>> DeactivateCustomer([FromRoute] int id)
    {
        if (!await customerService.ExistsByIdAsync(id))
        {
            return NotFound(new SimpleResponse("Cliente no encontrado"));
        }

        var userId = HttpContext.GetUserId();
        var responseMessage = await customerService.DeactivateAsync(id, userId);

        if (!responseMessage.Success)
        {
            return BadRequest(new SimpleResponse(responseMessage.Message));
        }

        return Ok(responseMessage.Data);
    }
}