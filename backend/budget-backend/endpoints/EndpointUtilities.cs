using System.Security.Claims;

namespace budget_backend.endpoints;

public class EndpointUtilities
{
    public static string ExtractAuthUserId(ClaimsPrincipal claims) => claims.FindFirstValue(ClaimTypes.NameIdentifier);
}