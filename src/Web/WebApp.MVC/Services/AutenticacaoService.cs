using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Api.Core.Usuario;
using Core.Communication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using WebApp.MVC.Extensions;
using WebApp.MVC.Models;

namespace WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAspNetUser _aspNetUser;

        public AutenticacaoService(HttpClient httpClient,
                                   IOptions<AppSettings> appSettings,
                                   IAuthenticationService authenticationService,
                                   IAspNetUser aspNetUser)
        {
            _httpClient = httpClient;
            _authenticationService = authenticationService;
            _aspNetUser = aspNetUser;
            if (string.IsNullOrEmpty(appSettings.Value.AutenticacaoUrl)) throw new Exception("Configuração da API de Autenticação não informada.");            
            _httpClient.BaseAddress = new Uri(appSettings.Value.AutenticacaoUrl);
        }

        public async Task<UsuarioRespostaLogin?> Login(UsuarioLogin usuarioLogin)
        {
            StringContent loginContent = ObterConteudo<UsuarioLogin>(usuarioLogin);
            var response = await _httpClient.PostAsync(requestUri: "/api/identidade/autenticar", content: loginContent);
            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin(responseResult: await DeserializarObjetoResponse<ResponseResult>(response));
            };
            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }

        public async Task<UsuarioRespostaLogin?> Registro(UsuarioRegistro usuarioRegistro)
        {
            StringContent usuarioRegistroContent = ObterConteudo<UsuarioRegistro>(usuarioRegistro);
            var response = await _httpClient.PostAsync(requestUri: "/api/identidade/nova-conta", content: usuarioRegistroContent);
            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin(responseResult: await DeserializarObjetoResponse<ResponseResult>(response));
            }
            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }
        
        public JwtSecurityToken? ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }

        public async Task Logout()
        {
            await _authenticationService.SignOutAsync(_aspNetUser.ObterHttpContext(), CookieAuthenticationDefaults.AuthenticationScheme, null);
        }
        
        public async Task RealizarLogin(UsuarioRespostaLogin usuarioRespostaLogin)
        {
            ArgumentNullException.ThrowIfNull(usuarioRespostaLogin.AccessToken, "Token Inválido");            
            
            JwtSecurityToken? token = ObterTokenFormatado(usuarioRespostaLogin.AccessToken);
            ArgumentNullException.ThrowIfNull(token, "Falha na formatação do Token.");    
            
            var claims = new List<Claim>();
            claims.Add(new Claim(type: "JWT", value: usuarioRespostaLogin.AccessToken));
            claims.Add(new Claim(type: "RefreshToken", value: usuarioRespostaLogin.RefreshToken));
            claims.AddRange(token.Claims);
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                IsPersistent = true
            };
            await _authenticationService.SignInAsync(_aspNetUser.ObterHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public bool TokenExpirado()
        {
            var jwt = _aspNetUser.ObterUserToken();
            if (jwt is null) return false;
            var token = ObterTokenFormatado(jwt);
            return token?.ValidTo.ToLocalTime() < DateTime.Now;
        }

        public async Task<UsuarioRespostaLogin> UtilizarRefreshToken(string refreshToken)
        {
            var refreshTokenContent = ObterConteudo(refreshToken);
            var response = await _httpClient.PostAsync("/api/identidade/refresh-token", refreshTokenContent);
            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response) ?? new UsuarioRespostaLogin();
        }

        public async Task<bool> RefreshTokenValido()
        {
            var resposta = await UtilizarRefreshToken(_aspNetUser.ObterUserRefreshToken() ?? string.Empty);
            if (resposta.AccessToken != null && resposta.ResponseResult == null)
            {
                await RealizarLogin(resposta);
                return true;
            }

            return false;
        }
    }
}