using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.API.Controllers
{
    [Route("api/cashregisters")]
    public class CashRegisterController(ICashRegisterService cashRegisterService) : ControllerBase
    {
        private readonly UserSession _session = new UserSession() { CompanyId = 2, UserId = 1, StoreId = 11 };

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetCashRegisters([FromQuery] bool includeInactives = false)
        {
            var cashRegisters = await cashRegisterService.GetAllAsync(_session.StoreId, includeInactives);
            return Ok(cashRegisters);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetCashRegisterById([FromRoute] int id)
        {
            var cashRegister = await cashRegisterService.GetByIdAsync(id, _session.StoreId);

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
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> AddCashRegister([FromBody] CashRegisterRequest request)
        {
            var responseMessage = await cashRegisterService.AddAsync(request, _session);

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
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateCashRegister([FromRoute] int id, [FromBody] CashRegisterRequest request)
        {
            if (!await cashRegisterService.ExistsByIdAsync(id, _session.StoreId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Caja no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await cashRegisterService.UpdateAsync(id, request, _session);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateCashRegister([FromRoute] int id)
        {
            if (!await cashRegisterService.ExistsByIdAsync(id, _session.StoreId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Caja no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await cashRegisterService.DeactivateAsync(id, _session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
