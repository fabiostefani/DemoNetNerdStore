using System.Text;
using System.Text.Json;
using WebApp.MVC.Models;

namespace WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
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
            if (!TratarErrosResponse(response))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var aa = await response.Content.ReadAsStringAsync();
                return new UsuarioRespostaLogin()
                {
                    ResponseResult = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), options)
                };                            
            };            
            return MontarRetorno(await response.Content.ReadAsStringAsync());
        }

        public async Task<UsuarioRespostaLogin?> Registro(UsuarioRegistro usuarioRegistro)
        {
            StringContent usuarioRegistroContent = MontarContent<UsuarioRegistro>(usuarioRegistro);            
            var response = await _httpClient.PostAsync(requestUri: "https://localhost:7002/api/identidade/nova-conta", content: usuarioRegistroContent);
            if (!TratarErrosResponse(response))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return new UsuarioRespostaLogin
                {
                    ResponseResult = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), options)
                };
            }
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