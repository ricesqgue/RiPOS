using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces
{
    public interface IStoreService
    {
        Task<ICollection<StoreResponse>> GetAllAsync();
        Task<StoreResponse?> GetByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<MessageResponse<StoreResponse>> AddAsync(StoreRequest request, int userId);
        Task<MessageResponse<StoreResponse>> UpdateAsync(int id, StoreRequest request, int userId);
        Task<MessageResponse<string>> DeactivateAsync(int id, int userId);
    }
}
