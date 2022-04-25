using System.Net.Http.Headers;
using WebApp.MVC.Extensions;

namespace WebApp.MVC.Services.Handlers;

public class HttpClientAuthorizationDelegationHandler : DelegatingHandler
{
    private readonly IUser _user;

    public HttpClientAuthorizationDelegationHandler(IUser user)
    {
        _user = user;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authorizationHeader = _user.ObterHttpContext().Request.Headers["Authorization"];
        if (!string.IsNullOrEmpty(authorizationHeader))
        {
            request.Headers.Add("Authorization", new List<string?>() {authorizationHeader});
        }
        string? token = _user.ObterUserToken();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        return base.SendAsync(request, cancellationToken);
    }
}