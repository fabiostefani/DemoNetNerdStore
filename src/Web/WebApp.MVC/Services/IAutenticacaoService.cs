using System.IdentityModel.Tokens.Jwt;
using WebApp.MVC.Models;

namespace WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        Task<UsuarioRespostaLogin?> Login(UsuarioLogin usuarioLogin);
        Task<UsuarioRespostaLogin?> Registro(UsuarioRegistro usuarioRegistro);
        JwtSecurityToken? ObterTokenFormatado(string jwtToken);
        Task Logout();
        Task RealizarLogin(UsuarioRespostaLogin usuarioRespostaLogin);
        bool TokenExpirado();
        Task<UsuarioRespostaLogin> UtilizarRefreshToken(string refreshToken);
        Task<bool> RefreshTokenValido();
    }
}