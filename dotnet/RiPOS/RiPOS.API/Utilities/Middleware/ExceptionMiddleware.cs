using System.Net;

namespace RiPOS.API.Utilities.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception e)
            {
                var log = $"{Environment.NewLine}Exception on: {httpContext.Request.Method} {httpContext.Request.Path} {Environment.NewLine}" +
                          $"Exception: {e} {Environment.NewLine} " +
                          $"{Environment.NewLine} ############################################################################################################# {Environment.NewLine} {Environment.NewLine}";

                logger.LogCritical(log);
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(exception.InnerException?.Message ?? exception.Message);
        }
    }
}
