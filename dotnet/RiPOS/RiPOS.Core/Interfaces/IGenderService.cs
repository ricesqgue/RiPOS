using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.Core.Interfaces
{
    public interface IGenderService
    {
        Task<ICollection<GenderResponse>> GetAllAsync(int companyId, bool includeInactives = false);
        Task<GenderResponse> GetByIdAsync(int id, int companyId);
        Task<bool> ExistsByIdAsync(int id, int companyId);
        Task<MessageResponse<GenderResponse>> AddAsync(GenderRequest request, UserSession userSession);
        Task<MessageResponse<GenderResponse>> UpdateAsync(int id, GenderRequest request, UserSession userSession);
        Task<MessageResponse<string>> DeactivateAsync(int id, UserSession userSession);
    }
}
