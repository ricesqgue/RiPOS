using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces
{
    public interface IVendorService
    {
        Task<ICollection<VendorResponse>> GetAllAsync(bool includeInactives = false);
        Task<VendorResponse> GetByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<MessageResponse<VendorResponse>> AddAsync(VendorRequest request, int userId);
        Task<MessageResponse<VendorResponse>> UpdateAsync(int id, VendorRequest request, int userId);
        Task<MessageResponse<string>> DeactivateAsync(int id, int userId);
    }
}
