using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces
{
    public interface IVendorService
    {
        Task<ICollection<VendorResponse>> GetAllAsync(int storeId, bool includeInactives = false);
        Task<VendorResponse> GetByIdAsync(int id, int storeId);
        Task<bool> ExistsByIdAsync(int id, int storeId);
        Task<MessageResponse<VendorResponse>> AddAsync(VendorRequest request, UserSession userSession);
        Task<MessageResponse<VendorResponse>> UpdateAsync(int id, VendorRequest request, UserSession userSession);
        Task<MessageResponse<string>> DeactivateAsync(int id, UserSession userSession);
    }
}
