using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Session;

namespace RiPOS.Core.Interfaces
{
    public interface ICashRegisterService
    {
        Task<ICollection<CashRegisterResponse>> GetAllAsync(int storeId, bool includeInactives = false);
        Task<CashRegisterResponse?> GetByIdAsync(int id, int storeId);
        Task<bool> ExistsByIdAsync(int id, int storeId);
        Task<MessageResponse<CashRegisterResponse>> AddAsync(CashRegisterRequest request, UserSession userSession);
        Task<MessageResponse<CashRegisterResponse>> UpdateAsync(int id, CashRegisterRequest request, UserSession userSession);
        Task<MessageResponse<string>> DeactivateAsync(int id, int userId);
    }
}
