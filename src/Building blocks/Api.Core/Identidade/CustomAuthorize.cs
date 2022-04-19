using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Core.Identidade
{
    public class CustomAuthorize
    {
        public static bool ValidarClaimsUsuario(HttpContext context, string clainName, string claimValue)
        {
            if (context.User.Identity == null)
                throw new Exception("context.User.Identity está nulo");
            return context.User.Identity.IsAuthenticated &&
                context.User.Claims.Any(x => x.Type == clainName && x.Value.Contains(claimValue));
        }
    }

    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string clainName, string claimValue)
            : base(typeof(RequisitoClaimFilter))
        {
            Arguments = new object[] { new Claim(clainName, claimValue) };
        }
    }

    public class RequisitoClaimFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;
        public RequisitoClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity == null)
                throw new Exception("context.HttpContext.User.Identity está nulo");            
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }
            if (!CustomAuthorize.ValidarClaimsUsuario(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}