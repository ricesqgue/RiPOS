using Microsoft.AspNetCore.Mvc;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Controllers
{
    [Route("api/company")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoreResponse>> GetCompany()
        {
            int id = 2;
            var company = await _companyService.GetByIdAsync(id);

            if (company == null)
            {
                var response = new MessageResponse<string>()
                {
                    Success = false,
                    Message = "Companía no encontrada"
                };
                return NotFound(response);
            }

            return Ok(company);
        }
    }
}
