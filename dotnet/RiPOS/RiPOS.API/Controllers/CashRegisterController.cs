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
    [Route("api/cashregisters")]
    [Authorize]
    public class CashRegisterController(ICashRegisterService cashRegisterService) : ControllerBase
    {
        
        [HttpGet]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ICollection<CashRegisterResponse>>> GetCashRegisters([FromQuery] bool includeInactives = false)
        {
            var storeId = ControllerContext.HttpContext.GetHeaderStoreId();
            var cashRegisters = await cashRegisterService.GetAllAsync(storeId, includeInactives);
            return Ok(cashRegisters);
        }

        [HttpGet("{id:int}")]
        [RoleAuthorize([RoleEnum.Admin])]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CashRegisterResponse>> GetCashRegisterById([FromRoute] int id)
        {
            var storeId = ControllerContext.HttpContext.GetHeaderStoreId();
            var cashRegister = await cashRegisterService.GetByIdAsync(id, storeId);

            if (cashRegister == null)
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Caja no encontrada"
                };
                return NotFound(response);
            }

            return Ok(cashRegister);
        }

        [HttpPost]
        [RoleAuthorize([RoleEnum.Admin])]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<MessageResponse<CashRegisterResponse>>> AddCashRegister([FromBody] CashRegisterRequest request)
        {
            var userSession = ControllerContext.HttpContext.GetUserSession();
            var responseMessage = await cashRegisterService.AddAsync(request, userSession);

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
        public async Task<ActionResult<MessageResponse<CashRegisterResponse>>> UpdateCashRegister([FromRoute] int id, [FromBody] CashRegisterRequest request)
        {
            var userSession = ControllerContext.HttpContext.GetUserSession();
            if (!await cashRegisterService.ExistsByIdAsync(id, userSession.StoreId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Caja no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await cashRegisterService.UpdateAsync(id, request, userSession);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateCashRegister([FromRoute] int id)
        {
            var userSession = ControllerContext.HttpContext.GetUserSession();
            if (!await cashRegisterService.ExistsByIdAsync(id, userSession.StoreId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Caja no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await cashRegisterService.DeactivateAsync(id, userSession.UserId);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
