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

[Route("api/categories")]
[Authorize]
[Consumes("application/json")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    [RoleAuthorize([RoleEnum.Admin])]
    [ProducesResponseType(typeof(ICollection<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<CategoryResponse>>> GetCategories([FromQuery] bool includeInactives = false)
    {
        var categories = await categoryService.GetAllAsync(includeInactives);
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    [RoleAuthorize([RoleEnum.Admin])]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryResponse>> GetCategoryById([FromRoute] int id)
    {
        var category = await categoryService.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound(new SimpleResponse("Categoría no encontrada"));
        }

        return Ok(category);
    }

    [HttpPost]
    [RoleAuthorize([RoleEnum.Admin])]
    [ModelValidation]
    [ProducesResponseType(typeof(MessageResponse<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MessageResponse<CategoryResponse>>> AddCategory([FromBody] CategoryRequest request)
    {
        var userId = HttpContext.GetUserId();
        var responseMessage = await categoryService.AddAsync(request, userId);

        if (!responseMessage.Success)
        {
            return BadRequest(new SimpleResponse(responseMessage.Message));
        }

        return Ok(responseMessage);
    }

    [HttpPut("{id}")]
    [RoleAuthorize([RoleEnum.Admin])]
    [ModelValidation]
    [ProducesResponseType(typeof(MessageResponse<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse<CategoryResponse>>> UpdateCategory([FromRoute] int id, [FromBody] CategoryRequest request)
    {
        if (!await categoryService.ExistsByIdAsync(id))
        {
            return NotFound(new SimpleResponse("Categoría no encontrada"));
        }

        var userId = HttpContext.GetUserId();
        var responseMessage = await categoryService.UpdateAsync(id, request, userId);

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
    public async Task<ActionResult<SimpleResponse>> DeactivateCategory([FromRoute] int id)
    {
        if (!await categoryService.ExistsByIdAsync(id))
        {
            return NotFound(new SimpleResponse("Categoría no encontrada"));
        }

        var userId = HttpContext.GetUserId();
        var responseMessage = await categoryService.DeactivateAsync(id, userId);

        if (!responseMessage.Success)
        {
            return BadRequest(new SimpleResponse(responseMessage.Message));
        }

        return Ok(responseMessage.Data);
    }
}