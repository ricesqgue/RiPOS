using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Controllers;

[Route("api/misc")]
[Authorize]
public class MiscController(IMiscService miscService) : ControllerBase
{
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [HttpGet("countryStates")]
    public async Task<ActionResult<ICollection<CountryStateResponse>>> GetCountryStates()
    {
        var countryStates = await miscService.GetAllCountryStatesAsync();
        return Ok(countryStates);
    }
}