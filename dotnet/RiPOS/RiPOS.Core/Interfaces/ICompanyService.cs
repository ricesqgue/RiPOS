using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponse> GetByIdAsync(int id);
    }
}
