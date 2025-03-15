using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<ICollection<CustomerResponse>> GetAllAsync(int storeId, bool includeInactives = false);
        Task<CustomerResponse> GetByIdAsync(int id, int storeId);
        Task<bool> ExistsByIdAsync(int id, int storeId);
        Task<MessageResponse<CustomerResponse>> AddAsync(CustomerRequest request, UserSession userSession);
        Task<MessageResponse<CustomerResponse>> UpdateAsync(int id, CustomerRequest request, UserSession userSession);
        Task<MessageResponse<string>> DeactivateAsync(int id, UserSession userSession);
    }
}
