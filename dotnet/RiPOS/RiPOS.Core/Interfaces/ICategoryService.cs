using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces;

public interface ICategoryService
{
    Task<ICollection<CategoryResponse>> GetAllAsync(bool includeInactives = false);
    Task<CategoryResponse?> GetByIdAsync(int id);
    Task<bool> ExistsByIdAsync(int id);
    Task<MessageResponse<CategoryResponse>> AddAsync(CategoryRequest request, int userId);
    Task<MessageResponse<CategoryResponse>> UpdateAsync(int id, CategoryRequest request, int userId);
    Task<MessageResponse<string>> DeactivateAsync(int id, int userId);
}