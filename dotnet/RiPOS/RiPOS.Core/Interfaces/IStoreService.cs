using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces
{
    public interface IStoreService
    {
        Task<ICollection<StoreResponse>> GetAllAsync(int companyId);
        Task<StoreResponse> GetByIdAsync(int id, int companyId);
        Task<bool> ExistsByIdAsync(int id, int companyId);
        Task<MessageResponse<StoreResponse>> AddAsync(StoreRequest request, UserSession userSession);
        Task<MessageResponse<StoreResponse>> UpdateAsync(int id, StoreRequest request, UserSession userSession);
        Task<MessageResponse<string>> DeactivateAsync(int id, UserSession userSession);
    }
}
