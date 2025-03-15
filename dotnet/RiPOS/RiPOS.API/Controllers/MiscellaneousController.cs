using Microsoft.AspNetCore.Mvc;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Controllers
{
    [Route("api/miscellaneous")]

    public class MiscellaneousController : ControllerBase
    {
        private readonly IMiscellaneousService _miscellaneousService;

        public MiscellaneousController(IMiscellaneousService miscellaneousService)
        {
            _miscellaneousService = miscellaneousService;
        }

        [ProducesResponseType(200)]
        [HttpGet("countryStates")]
        public async Task<ActionResult<ICollection<CountryStateResponse>>> GetCountryStates()
        {
            var countryStates = await _miscellaneousService.GetAllCountryStatesAsync();
            return Ok(countryStates);
        }
    }
}
