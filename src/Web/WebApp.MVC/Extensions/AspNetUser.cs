using System.Security.Principal;
using System;
using System.Security.Claims;

namespace WebApp.MVC.Extensions
{
    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;
        public AspNetUser(IHttpContextAccessor httpContextAccessor)
        {
            _accessor = httpContextAccessor;
        }
        public string? Name => _accessor?.HttpContext?.User?.Identity?.Name;
        
        public bool EstaAutenticado() 
        {
            return _accessor.ValidarHttpContext()
            .ValidarUser()
            .ValidarUserIdentity()
            .EstaAutenticado();
        }
        

        public IEnumerable<Claim>? ObterClaims()
        {
            return _accessor
            .ValidarHttpContext()
            .ValidarUser()
            .ObterClaims();            
        }

        public HttpContext ObterHttpContext()
        {
            return _accessor
            .ValidarHttpContext()
            .ObterHttpContext();            
        }

        public string? ObterUserEmail()
        {
            return EstaAutenticado()
                ? _accessor?.HttpContext?.User.GetUserEmail()
                : string.Empty;
        }

        public Guid? ObterUserId()
        {
            var guidUserId = _accessor?.HttpContext?.User?.GetUserId();
            if (guidUserId == null)
                throw new Exception("Erro recuperando o Guid do usuário");            
            return EstaAutenticado()
                ? Guid.Parse(guidUserId)
                : Guid.Empty;
        }

        public string? ObterUserToken()
        {
            return EstaAutenticado()
                ? _accessor?.HttpContext?.User?.GetUserToken()
                : string.Empty;
        }

        public bool? PossuiRole(string role)
        {
            return _accessor?.HttpContext?.User?.IsInRole(role);
        }
    }
}