using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.Core.Interfaces
{
    public interface ISizeService
    {
        Task<ICollection<SizeResponse>> GetAllAsync(int companyId, bool includeInactives = false);
        Task<SizeResponse> GetByIdAsync(int id, int companyId);
        Task<bool> ExistsByIdAsync(int id, int companyId);
        Task<MessageResponse<SizeResponse>> AddAsync(SizeRequest request, UserSession userSession);
        Task<MessageResponse<SizeResponse>> UpdateAsync(int id, SizeRequest request, UserSession userSession);
        Task<MessageResponse<string>> DeactivateAsync(int id, UserSession userSession);
    }
}
