using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.API.Controllers
{
    [Route("api/categories")]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        private readonly UserSession _session = new UserSession() { CompanyId = 2, UserId = 1 };

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetCategories([FromQuery] bool includeInactives = false)
        {
            var categories = await categoryService.GetAllAsync(_session.CompanyId, includeInactives);
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetCategoryById([FromRoute] int id)
        {
            var category = await categoryService.GetByIdAsync(id, _session.CompanyId);

            if (category == null)
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Categoría no encontrada"
                };
                return NotFound(response);
            }

            return Ok(category);
        }

        [HttpPost]
        [ModelValidation]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MessageResponse<StoreResponse>>> AddCategory([FromBody] CategoryRequest request)
        {
            var responseMessage = await categoryService.AddAsync(request, _session);

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
        public async Task<ActionResult<MessageResponse<StoreResponse>>> UpdateCategory([FromRoute] int id, [FromBody] CategoryRequest request)
        {
            if (!await categoryService.ExistsByIdAsync(id, _session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Categoría no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await categoryService.UpdateAsync(id, request, _session);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateCategory([FromRoute] int id)
        {
            if (!await categoryService.ExistsByIdAsync(id, _session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Categoría no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await categoryService.DeactivateAsync(id, _session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
