using System.Security.Claims;
using System.Security.Principal;

namespace WebApp.MVC.Extensions
{
    public static class IHttpContextAccessorExtensions
    {
        public static HttpContext ValidarHttpContext(this IHttpContextAccessor accessor)
        {
            if (accessor.HttpContext == null)
                throw new Exception("HttpContext está nulo");
            return accessor.HttpContext;
        }

        public static ClaimsPrincipal ValidarUser(this HttpContext httpContext)
        {
            if (httpContext.User == null)
                throw new Exception("User está nulo");
            return httpContext.User;
        }

        public static IIdentity ValidarUserIdentity(this ClaimsPrincipal user)
        {
            if (user.Identity == null)
                throw new Exception("Identity está nulo");
            return user.Identity;
        }

        public static bool EstaAutenticado(this IIdentity identity)
        {
            return identity.IsAuthenticated;
        }

        public static IEnumerable<Claim> ObterClaims(this ClaimsPrincipal user)
        {
            return user.Claims;
        }

        public static HttpContext ObterHttpContext(this HttpContext httpContext)
        {
            return httpContext;
        }

        public static string ObterUserEmail(this ClaimsPrincipal user)
        {
            return user.GetUserEmail();
        }

        public static string ObterUserId(this ClaimsPrincipal user)
        {
            return user.GetUserId();            
        }

        public static string ObterUserToken(this ClaimsPrincipal user)
        {
            return user.GetUserToken();
        }

        public static bool PossuiRole(this ClaimsPrincipal user, string role)
        {
            return user.IsInRole(role);
        }
    }
}