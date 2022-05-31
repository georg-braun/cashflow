using System.Security.Claims;

namespace budget_backend.endpoints;

public class EndpointUtilities
{
    public static string ExtractAuthUserId(ClaimsPrincipal claims)
    {
        return claims.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}