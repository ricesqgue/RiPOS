using RiPOS.Domain.Entities;

namespace RiPOS.Repository.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company> GetByIdAsync(int id);
    }
}
