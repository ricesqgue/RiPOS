using RiPOS.Domain.Entities;

namespace RiPOS.Repository.Interfaces
{
    public interface IMiscellaneousRepository
    {
        Task<ICollection<CountryState>> GetAllCountryStatesAsync();
    }
}
