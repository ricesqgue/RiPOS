using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Session;

namespace RiPOS.Core.Interfaces
{
    public interface IBrandService
    {
        Task<ICollection<BrandResponse>> GetAllAsync(bool includeInactives = false);
        Task<BrandResponse> GetByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<MessageResponse<BrandResponse>> AddAsync(BrandRequest request, int userId);
        Task<MessageResponse<BrandResponse>> UpdateAsync(int id, BrandRequest request, int userId);
        Task<MessageResponse<string>> DeactivateAsync(int id, int userId);
    }
}
