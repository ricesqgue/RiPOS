using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.API.Controllers
{
    [Route("api/cashregisters")]
    public class CashRegisterController : ControllerBase
    {
        private readonly ICashRegisterService _cashRegisterService;
        private readonly UserSession session = new UserSession() { CompanyId = 2, UserId = 1, StoreId = 11 };

        public CashRegisterController(ICashRegisterService cashRegisterService)
        {
            _cashRegisterService = cashRegisterService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetCashRegisters([FromQuery] bool includeInactives = false)
        {
            var cashRegisters = await _cashRegisterService.GetAllAsync(session.StoreId, includeInactives);
            return Ok(cashRegisters);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetCashRegisterById([FromRoute] int id)
        {
            var cashRegister = await _cashRegisterService.GetByIdAsync(id, session.StoreId);

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
            var responseMessage = await _cashRegisterService.AddAsync(request, session);

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
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateCashRegister([FromRoute] int id, [FromBody] CashRegisterRequest request)
        {
            if (!await _cashRegisterService.ExistsByIdAsync(id, session.StoreId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Caja no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await _cashRegisterService.UpdateAsync(id, request, session);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateCashRegister([FromRoute] int id)
        {
            if (!await _cashRegisterService.ExistsByIdAsync(id, session.StoreId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Caja no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await _cashRegisterService.DeactivateAsync(id, session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
