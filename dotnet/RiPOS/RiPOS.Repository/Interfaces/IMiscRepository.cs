using RiPOS.Domain.Entities;

namespace RiPOS.Repository.Interfaces
{
    public interface IMiscRepository
    {
        Task<ICollection<CountryState>> GetAllCountryStatesAsync();
    }
}
