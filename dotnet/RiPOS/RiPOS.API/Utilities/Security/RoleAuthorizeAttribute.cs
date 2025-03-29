using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RiPOS.API.Utilities.Extensions;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Enums;

namespace RiPOS.API.Utilities.Security;

public class RoleAuthorizeAttribute(RoleEnum[] allowedRoles) : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private const string MemoryCacheKey = "user_roles_store";
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var storeId = context.HttpContext.GetHeaderStoreId();
        var userId = context.HttpContext.GetUserId();
        var userService = context.HttpContext.RequestServices.GetService<IUserService>();
        var memoryCache = context.HttpContext.RequestServices.GetService<IMemoryCache>();
        
        if (userId == 0)
        {
            context.Result = new UnauthorizedObjectResult("Unauthorized");
            return;
        }
        
        var userMemoryCacheKey = $"{MemoryCacheKey}_{storeId}_{userId}";

        if (!memoryCache.TryGetValue(userMemoryCacheKey, out ICollection<RoleEnum> userRoles))
        {
            userRoles = await userService.GetUserRolesByStoreIdAsync(storeId, userId);
            if (userRoles.Count == 0)
            {
                context.Result = new ForbidResult();
                return;
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
            memoryCache.Set(userMemoryCacheKey, userRoles, cacheEntryOptions);
        }
        
        if (!allowedRoles.Any(r => userRoles.Contains(r)) && !userRoles.Contains(RoleEnum.SuperAdmin))
        {
            context.Result = new ForbidResult();
        }
    }
}