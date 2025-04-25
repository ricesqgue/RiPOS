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

[Route("api/cashregisters")]
[Authorize]
[Consumes("application/json")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class CashRegisterController(ICashRegisterService cashRegisterService) : ControllerBase
{
        
    [HttpGet]
    [RoleAuthorize([RoleEnum.Admin])]
    [ProducesResponseType(typeof(ICollection<CashRegisterResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<CashRegisterResponse>>> GetCashRegisters([FromQuery] bool includeInactives = false)
    {
        var storeId = ControllerContext.HttpContext.GetHeaderStoreId();
        var cashRegisters = await cashRegisterService.GetAllAsync(storeId, includeInactives);
        return Ok(cashRegisters);
    }

    [HttpGet("{id:int}")]
    [RoleAuthorize([RoleEnum.Admin])]
    [ProducesResponseType(typeof(CashRegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CashRegisterResponse>> GetCashRegisterById([FromRoute] int id)
    {
        var storeId = ControllerContext.HttpContext.GetHeaderStoreId();
        var cashRegister = await cashRegisterService.GetByIdAsync(id, storeId);

        if (cashRegister == null)
        {
            return NotFound(new SimpleResponse("Caja no encontrada"));
        }

        return Ok(cashRegister);
    }

    [HttpPost]
    [RoleAuthorize([RoleEnum.Admin])]
    [ModelValidation]
    [ProducesResponseType(typeof(MessageResponse<CashRegisterResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MessageResponse<CashRegisterResponse>>> AddCashRegister([FromBody] CashRegisterRequest request)
    {
        var userSession = ControllerContext.HttpContext.GetUserSession();
        var responseMessage = await cashRegisterService.AddAsync(request, userSession);

        if (!responseMessage.Success)
        {
            return BadRequest(new SimpleResponse(responseMessage.Message));
        }

        return Ok(responseMessage);
    }

    [HttpPut("{id:int}")]
    [RoleAuthorize([RoleEnum.Admin])]
    [ModelValidation]
    [ProducesResponseType(typeof(MessageResponse<CashRegisterResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse<CashRegisterResponse>>> UpdateCashRegister([FromRoute] int id, [FromBody] CashRegisterRequest request)
    {
        var userSession = ControllerContext.HttpContext.GetUserSession();
        if (!await cashRegisterService.ExistsByIdAsync(id, userSession.StoreId))
        {
            return NotFound(new SimpleResponse("Caja no encontrada"));
        }

        var responseMessage = await cashRegisterService.UpdateAsync(id, request, userSession);

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
    public async Task<ActionResult<SimpleResponse>> DeactivateCashRegister([FromRoute] int id)
    {
        var userSession = ControllerContext.HttpContext.GetUserSession();
        if (!await cashRegisterService.ExistsByIdAsync(id, userSession.StoreId))
        {
            return NotFound(new SimpleResponse("Caja no encontrada"));
        }

        var responseMessage = await cashRegisterService.DeactivateAsync(id, userSession.UserId);

        if (!responseMessage.Success)
        {
            return BadRequest(new SimpleResponse(responseMessage.Message));
        }

        return Ok(new SimpleResponse(responseMessage.Data));
    }
}