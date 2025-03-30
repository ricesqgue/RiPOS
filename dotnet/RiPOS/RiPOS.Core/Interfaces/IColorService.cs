using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces
{
    public interface IColorService
    {
        Task<ICollection<ColorResponse>> GetAllAsync(bool includeInactives = false);
        Task<ColorResponse?> GetByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<MessageResponse<ColorResponse>> AddAsync(ColorRequest request, int userId);
        Task<MessageResponse<ColorResponse>> UpdateAsync(int id, ColorRequest request, int userId);
        Task<MessageResponse<string>> DeactivateAsync(int id, int userId);
    }
}
