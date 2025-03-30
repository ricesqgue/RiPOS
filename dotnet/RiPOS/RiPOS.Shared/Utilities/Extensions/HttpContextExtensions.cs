using Microsoft.AspNetCore.Http;
using RiPOS.Shared.Models.Session;

namespace RiPOS.Shared.Utilities.Extensions;

public static class HttpContextExtensions
{
    private const string StoreIdHeader = "X-Store-Id";
    
    public static int GetHeaderStoreId(this HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(StoreIdHeader, out var storeId))
        {
            if (int.TryParse(storeId, out var id))
            {
                return id;
            }
        }
        return 0;
    }
    
    public static int GetUserId(this HttpContext context)
    {
        var userIdValue = context.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (userIdValue != null)
        {
            return int.Parse(userIdValue);
        }
        return 0;
    }

    public static UserSession GetUserSession(this HttpContext context)
    {
        var userSession = new UserSession()
        {
            UserId = context.GetUserId(),
            StoreId = context.GetHeaderStoreId(),
        };

        return userSession;
    }
}