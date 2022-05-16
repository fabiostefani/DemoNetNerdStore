using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Api.Core.Usuario;

public class AspNetUser : IAspNetUser
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
            ? _accessor.ValidarHttpContext().ValidarUser().GetUserEmail()
            : string.Empty;
    }

    public Guid? ObterUserId()
    {
        string guidUserId = _accessor.ValidarHttpContext().ValidarUser().GetUserId();
        if (string.IsNullOrEmpty(guidUserId))
            throw new Exception("Erro recuperando o Guid do usuário");            
        return EstaAutenticado()
            ? Guid.Parse(guidUserId)
            : Guid.Empty;
    }

    public string? ObterUserToken()
    {
        return EstaAutenticado()
            ? _accessor.ValidarHttpContext().ValidarUser().GetUserToken()
            : string.Empty;
    }

    public bool? PossuiRole(string role)
    {
        return _accessor
            .ValidarHttpContext()
            .ValidarUser()
            .IsInRole(role);
    }
}