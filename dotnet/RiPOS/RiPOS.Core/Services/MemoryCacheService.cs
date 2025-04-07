using Microsoft.Extensions.Caching.Memory;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Enums;

namespace RiPOS.Core.Services;

public class MemoryCacheService(IMemoryCache memoryCache) : IMemoryCacheService
{
    private const string MemoryCacheUserStoreRolesPrefixKey = "user_store_roles";

    public ICollection<RoleEnum> GetUserStoreRoles(int userId, int storeId)
    {
        var userMemoryCacheKey = $"{MemoryCacheUserStoreRolesPrefixKey}_{storeId}_{userId}";
        if (memoryCache.TryGetValue(userMemoryCacheKey, out ICollection<RoleEnum>? roles))
        {
            return roles ?? new List<RoleEnum>();
        }

        return new List<RoleEnum>();
    }

    public void SetUserStoreRoles(int userId, int storeId, ICollection<RoleEnum> roles)
    {
        var userMemoryCacheKey = $"{MemoryCacheUserStoreRolesPrefixKey}_{storeId}_{userId}";
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
        memoryCache.Set(userMemoryCacheKey, roles, cacheEntryOptions);
    }
}