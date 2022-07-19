using System.Security.Claims;

namespace Api.Core.Usuario;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentException(nameof(principal));
        }
        var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
            throw new Exception("Erro obtendo o Claim UserId");            
        return claim.Value;
    }

    public static string? GetUserEmail(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentException(nameof(principal));
        }
        var claim = principal.FindFirst("email");
        return claim?.Value;
    }

    public static string? GetUserToken(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentException(nameof(principal));
        }
        var claim = principal.FindFirst("JWT");
        return claim?.Value;
    }

    public static string? GetUserRefreshToken(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentException(nameof(principal));
        }

        var claim = principal.FindFirst("RefreshToken");
        return claim?.Value;
    }
}