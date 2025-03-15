using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.Core.Interfaces
{
    public interface IColorService
    {
        Task<ICollection<ColorResponse>> GetAllAsync(int companyId, bool includeInactives = false);
        Task<ColorResponse> GetByIdAsync(int id, int companyId);
        Task<bool> ExistsByIdAsync(int id, int companyId);
        Task<MessageResponse<ColorResponse>> AddAsync(ColorRequest request, UserSession userSession);
        Task<MessageResponse<ColorResponse>> UpdateAsync(int id, ColorRequest request, UserSession userSession);
        Task<MessageResponse<string>> DeactivateAsync(int id, UserSession userSession);
    }
}
