using Microsoft.AspNetCore.Mvc;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Controllers
{
    [Route("api/misc")]

    public class MiscController(IMiscellaneousService miscService) : ControllerBase
    {
        [ProducesResponseType(200)]
        [HttpGet("countryStates")]
        public async Task<ActionResult<ICollection<CountryStateResponse>>> GetCountryStates()
        {
            var countryStates = await miscService.GetAllCountryStatesAsync();
            return Ok(countryStates);
        }
    }
}
