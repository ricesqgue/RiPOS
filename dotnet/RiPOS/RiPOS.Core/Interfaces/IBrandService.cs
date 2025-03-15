using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.Core.Interfaces
{
    public interface IBrandService
    {
        Task<ICollection<BrandResponse>> GetAllAsync(int companyId, bool includeInactives = false);
        Task<BrandResponse> GetByIdAsync(int id, int companyId);
        Task<bool> ExistsByIdAsync(int id, int companyId);
        Task<MessageResponse<BrandResponse>> AddAsync(BrandRequest request, UserSession userSession);
        Task<MessageResponse<BrandResponse>> UpdateAsync(int id, BrandRequest request, UserSession userSession);
        Task<MessageResponse<string>> DeactivateAsync(int id, UserSession userSession);
    }
}
