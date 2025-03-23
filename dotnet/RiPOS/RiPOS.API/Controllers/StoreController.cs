using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Controllers
{
    [Route("api/stores")]
    public class StoreController(IStoreService storeService) : ControllerBase
    {
        private readonly UserSession _session = new UserSession() { CompanyId = 2, UserId = 1 };

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetStores()
        {
            var stores = await storeService.GetAllAsync(_session.CompanyId);
            return Ok(stores);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetStoreById([FromRoute] int id)
        {
            var store = await storeService.GetByIdAsync(id, _session.CompanyId);

            if (store == null)
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Tienda no encontrada"
                };
                return NotFound(response);
            }

            return Ok(store);
        }

        [HttpPost]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> AddStore([FromBody] StoreRequest request)
        {
            var responseMessage = await storeService.AddAsync(request, _session);

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
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateStore([FromRoute] int id, [FromBody] StoreRequest request)
        {
            if (!await storeService.ExistsByIdAsync(id, _session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Tienda no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await storeService.UpdateAsync(id, request, _session);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateStore([FromRoute] int id)
        {
            if (!await storeService.ExistsByIdAsync(id, _session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Tienda no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await storeService.DeactivateAsync(id, _session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
