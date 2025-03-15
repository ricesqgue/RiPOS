using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces
{
    public interface IMiscellaneousService
    {
        Task<ICollection<CountryStateResponse>> GetAllCountryStatesAsync();
    }
}
