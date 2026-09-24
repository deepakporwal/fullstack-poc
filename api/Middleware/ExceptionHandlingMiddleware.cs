using System.Net;
using System.Text.Json;

namespace FullStackPoc.Api.Middleware;
public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try { await next(context); }
        catch (Exception ex)
        {
            var traceId = context.TraceIdentifier;
            logger.LogError(ex, "Unhandled API exception. TraceId={TraceId}, Path={Path}, Method={Method}", traceId, context.Request.Path, context.Request.Method);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { statusCode = 500, message = "An unexpected error occurred.", traceId }));
        }
    }
}
