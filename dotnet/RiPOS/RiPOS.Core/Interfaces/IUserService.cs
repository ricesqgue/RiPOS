using RiPOS.Shared.Enums;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces
{
    public interface IUserService
    {
        Task<ICollection<UserResponse>> GetAllByCompanyAsync(int companyId, bool includeInactives = false);
        Task<ICollection<UserResponse>> GetAllByStoreAsync(int storeId, bool includeInactives = false);
        Task<UserResponse> GetByIdInCompanyAsync(int id, int companyId);
        Task<UserResponse> GetByIdInStoreAsync(int id, int storeId);
        Task<bool> ExistsByIdInCompanyAsync(int id, int companyId);
        Task<bool> ExistsByIdInStoreAsync(int id, int storeId);
        Task<ICollection<RoleEnum>> GetUserRolesByStoreIdAsync(int userId, int storeId);
    }
}
