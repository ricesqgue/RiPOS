using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RiPOS.API.Utilities.Extensions;
using RiPOS.Core.Interfaces;
using RiPOS.Shared.Enums;
using RiPOS.Shared.Utilities.Extensions;

namespace RiPOS.API.Utilities.Security;

public class RoleAuthorizeAttribute(RoleEnum[] allowedRoles) : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var storeId = context.HttpContext.GetHeaderStoreId();
        var userId = context.HttpContext.GetUserId();
        var userService = context.HttpContext.RequestServices.GetService<IUserService>();
        var memoryCacheService = context.HttpContext.RequestServices.GetService<IMemoryCacheService>();

        if (userService == null || memoryCacheService == null)
        {
            context.Result = new UnauthorizedObjectResult("Unable to get services");
            return;
        }
        
        if (userId == 0)
        {
            context.Result = new UnauthorizedObjectResult("Unauthorized");
            return;
        }
        
        var userRoles = memoryCacheService.GetUserStoreRoles(userId, storeId);

        if (userRoles.Count == 0)
        {
            userRoles = await userService.GetUserRolesByStoreIdAsync(userId, storeId);
            if (userRoles.Count == 0)
            {
                context.Result = new ForbidResult();
                return;
            }
            memoryCacheService.SetUserStoreRoles(userId, storeId, userRoles);
        }
        
        if (!allowedRoles.Any(r => userRoles.Contains(r)) && !userRoles.Contains(RoleEnum.SuperAdmin))
        {
            context.Result = new ForbidResult();
        }
    }
}