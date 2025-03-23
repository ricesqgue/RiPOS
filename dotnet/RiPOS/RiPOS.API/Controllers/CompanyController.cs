using Microsoft.AspNetCore.Mvc;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Controllers
{
    [Route("api/company")]
    public class CompanyController(ICompanyService companyService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetCompany()
        {
            int id = 2;
            var company = await companyService.GetByIdAsync(id);

            if (company == null)
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Compañía no encontrada"
                };
                return NotFound(response);
            }

            return Ok(company);
        }
    }
}
