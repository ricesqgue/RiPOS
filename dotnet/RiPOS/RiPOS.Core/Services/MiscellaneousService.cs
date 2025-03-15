using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Services
{
    public class MiscellaneousService : IMiscellaneousService
    {
        private readonly IMapper _mapper;
        private readonly IMiscellaneousRepository _repository;

        public MiscellaneousService(IMapper mapper, IMiscellaneousRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ICollection<CountryStateResponse>> GetAllCountryStatesAsync()
        {
            var countryStates = await _repository.GetAllCountryStatesAsync();
            var countryStatesResponse = _mapper.Map<ICollection<CountryStateResponse>>(countryStates);
            return countryStatesResponse;
        }
    }
}
