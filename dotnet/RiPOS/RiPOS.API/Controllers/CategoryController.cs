using Microsoft.AspNetCore.Mvc;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.API.Controllers
{
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly UserSession session = new UserSession() { CompanyId = 2, UserId = 1 };

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ICollection<StoreResponse>>> GetCategories([FromQuery] bool includeInactives = false)
        {
            var categories = await _categoryService.GetAllAsync(session.CompanyId, includeInactives);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetCategoryById([FromRoute] int id)
        {
            var category = await _categoryService.GetByIdAsync(id, session.CompanyId);

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
            var responseMessage = await _categoryService.AddAsync(request, session);

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
            if (!await _categoryService.ExistsByIdAsync(id, session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Categoría no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await _categoryService.UpdateAsync(id, request, session);

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
        public async Task<ActionResult<MessageResponse<string>>> DeactivateCategory([FromRoute] int id)
        {
            if (!await _categoryService.ExistsByIdAsync(id, session.CompanyId))
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Categoría no encontrada"
                };
                return NotFound(response);
            }

            var responseMessage = await _categoryService.DeactivateAsync(id, session);

            if (!responseMessage.Success)
            {
                return BadRequest(responseMessage);
            }

            return Ok(responseMessage);
        }
    }
}
