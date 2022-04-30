namespace budget_backend.middleware;

public class DebugAllRequestsMiddleware
{
    private readonly RequestDelegate _next;

    public DebugAllRequestsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}

public static class DebugAllRequestsMiddlewareExtensions
{
    public static IApplicationBuilder UseDebugAllRequestsMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DebugAllRequestsMiddleware>();
    }
}