using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<ICollection<CategoryResponse>> GetAllAsync(int storeId, bool includeInactives = false);
        Task<CategoryResponse> GetByIdAsync(int id, int storeId);
        Task<bool> ExistsByIdAsync(int id, int storeId);
        Task<MessageResponse<CategoryResponse>> AddAsync(CategoryRequest request, UserSession userSession);
        Task<MessageResponse<CategoryResponse>> UpdateAsync(int id, CategoryRequest request, UserSession userSession);
        Task<MessageResponse<string>> DeactivateAsync(int id, UserSession userSession);
    }
}
