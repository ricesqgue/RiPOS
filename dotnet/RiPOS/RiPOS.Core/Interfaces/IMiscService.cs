using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces
{
    public interface IMiscService
    {
        Task<ICollection<CountryStateResponse>> GetAllCountryStatesAsync();
    }
}
