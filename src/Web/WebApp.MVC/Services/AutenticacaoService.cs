using System.Text;
using System.Text.Json;
using WebApp.MVC.Models;

namespace WebApp.MVC.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly HttpClient _httpClient;

        public AutenticacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UsuarioRespostaLogin?> Login(UsuarioLogin usuarioLogin)
        {
            StringContent loginContent = MontarContent<UsuarioLogin>(usuarioLogin);
            var response = await _httpClient.PostAsync(requestUri: "https://localhost:7002/api/identidade/autenticar", content: loginContent);            
            return MontarRetorno(await response.Content.ReadAsStringAsync());
        }

        public async Task<UsuarioRespostaLogin?> Registro(UsuarioRegistro usuarioRegistro)
        {
            StringContent usuarioRegistroContent = MontarContent<UsuarioRegistro>(usuarioRegistro);            
            var response = await _httpClient.PostAsync(requestUri: "https://localhost:7002/api/identidade/nova-conta", content: usuarioRegistroContent);
            return MontarRetorno(await response.Content.ReadAsStringAsync());
        }

        private static StringContent MontarContent<T>(T dados)
        {
            return new StringContent(
                content: JsonSerializer.Serialize(dados),
                Encoding.UTF8,
                mediaType: "application/json"
            );
        }

        private static UsuarioRespostaLogin? MontarRetorno(string dadosRetornar)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<UsuarioRespostaLogin>(dadosRetornar, options);
        }
    }
}