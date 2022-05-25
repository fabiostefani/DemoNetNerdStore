using System.Text;
using System.Text.Json;
using Core.Communication;
using WebApp.MVC.Core;
using WebApp.MVC.Extensions;
using WebApp.MVC.Models;

namespace WebApp.MVC.Services
{
    public abstract class Service
    {
        protected StringContent ObterConteudo<T>(T dados)
        {
            return new StringContent(
                content: JsonSerializer.Serialize(dados),
                Encoding.UTF8,
                mediaType: "application/json"
            );
        }

        protected async Task<T?> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var msg = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(msg, options);
        }

        protected bool TratarErrosResponse(HttpResponseMessage responseMessage)
        {
            switch ((int)responseMessage.StatusCode)
            {
                case InternalStatusCode.Unauthorized:
                case InternalStatusCode.Forbidden:
                case InternalStatusCode.NotFound:
                case InternalStatusCode.InternalServerError:
                    throw new CustomHttpRequestException(responseMessage.StatusCode);
                case InternalStatusCode.BadRequest:
                    return false;                
            }

            responseMessage.EnsureSuccessStatusCode();
            return true;
        }

        protected ResponseResult? RetornoOk()
            => new ResponseResult();
    }
}