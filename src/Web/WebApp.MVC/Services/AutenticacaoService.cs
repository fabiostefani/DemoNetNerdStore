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
            StringContent loginContent = ObterConteudo<UsuarioLogin>(usuarioLogin);
            var response = await _httpClient.PostAsync(requestUri: "https://localhost:7002/api/identidade/autenticar", content: loginContent);
            if (!TratarErrosResponse(response))
            {                
                return new UsuarioRespostaLogin(responseResult: await DeserializarObjetoResponse<ResponseResult>(response));                            
            };            
            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }

        public async Task<UsuarioRespostaLogin?> Registro(UsuarioRegistro usuarioRegistro)
        {
            StringContent usuarioRegistroContent = ObterConteudo<UsuarioRegistro>(usuarioRegistro);            
            var response = await _httpClient.PostAsync(requestUri: "https://localhost:7002/api/identidade/nova-conta", content: usuarioRegistroContent);
            if (!TratarErrosResponse(response))
            {                
                return new UsuarioRespostaLogin(responseResult: await DeserializarObjetoResponse<ResponseResult>(response));                            
            }
            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }
    }
}