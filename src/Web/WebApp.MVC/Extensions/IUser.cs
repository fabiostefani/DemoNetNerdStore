using System.Security.Claims;

namespace WebApp.MVC.Extensions
{
    public interface IUser
    {
        string? Name { get; }
        Guid? ObterUserId();
        string? ObterUserEmail();
        string? ObterUserToken();
        bool EstaAutenticado();
        bool? PossuiRole(string role);
        IEnumerable<Claim>? ObterClaims();
        HttpContext ObterHttpContext();
    }
}