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
        var claim = principal.FindFirst("sub");
        if (claim == null)
            throw new Exception("Erro obtendo o Claim UserId");            
        return claim.Value;
    }

    public static string GetUserEmail(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentException(nameof(principal));
        }
        var claim = principal.FindFirst("email");
        if (claim == null)
            throw new Exception("Erro obtendo o Claim Email");            
        return claim.Value;
    }

    public static string GetUserToken(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentException(nameof(principal));
        }
        var claim = principal.FindFirst("JWT");
        if (claim == null)
            throw new Exception("Erro obtendo o Claim JWT");            
        return claim.Value;
    }
}