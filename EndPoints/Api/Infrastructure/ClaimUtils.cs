using System.Security.Claims;

namespace Api.Infrastructure;

public static class ClaimUtils
{
    public static int GetUserId(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");
    }
    public static string GetPhoneNumber(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return principal.FindFirst(ClaimTypes.MobilePhone)?.Value ?? "";
    }
}