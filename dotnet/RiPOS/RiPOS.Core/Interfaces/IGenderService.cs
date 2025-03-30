using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.Core.Interfaces
{
    public interface IGenderService
    {
        Task<ICollection<GenderResponse>> GetAllAsync(bool includeInactives = false);
        Task<GenderResponse?> GetByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<MessageResponse<GenderResponse>> AddAsync(GenderRequest request, int userId);
        Task<MessageResponse<GenderResponse>> UpdateAsync(int id, GenderRequest request, int userId);
        Task<MessageResponse<string>> DeactivateAsync(int id, int userId);
    }
}
