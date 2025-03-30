using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<ICollection<CustomerResponse>> GetAllAsync(bool includeInactives = false);
        Task<CustomerResponse?> GetByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<MessageResponse<CustomerResponse>> AddAsync(CustomerRequest request, int userId);
        Task<MessageResponse<CustomerResponse>> UpdateAsync(int id, CustomerRequest request, int userId);
        Task<MessageResponse<string>> DeactivateAsync(int id, int userId);
    }
}
