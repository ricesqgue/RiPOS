using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.Core.Interfaces
{
    public interface ISizeService
    {
        Task<ICollection<SizeResponse>> GetAllAsync(bool includeInactives = false);
        Task<SizeResponse> GetByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<MessageResponse<SizeResponse>> AddAsync(SizeRequest request, int userId);
        Task<MessageResponse<SizeResponse>> UpdateAsync(int id, SizeRequest request, int userId);
        Task<MessageResponse<string>> DeactivateAsync(int id, int userId);
    }
}
