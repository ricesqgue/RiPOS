using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Services
{
    public class MiscellaneousService(IMapper mapper, IMiscellaneousRepository repository) : IMiscellaneousService
    {
        public async Task<ICollection<CountryStateResponse>> GetAllCountryStatesAsync()
        {
            var countryStates = await repository.GetAllCountryStatesAsync();
            var countryStatesResponse = mapper.Map<ICollection<CountryStateResponse>>(countryStates);
            return countryStatesResponse;
        }
    }
}
