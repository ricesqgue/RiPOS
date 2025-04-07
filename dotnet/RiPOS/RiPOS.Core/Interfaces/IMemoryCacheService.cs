using RiPOS.Shared.Enums;

namespace RiPOS.Core.Interfaces;

public interface IMemoryCacheService
{
    ICollection<RoleEnum> GetUserStoreRoles(int userId, int storeId);

    void SetUserStoreRoles(int userId, int storeId, ICollection<RoleEnum> roles);
}