using System.Net;

namespace BFF.Compras.Services;
using System.Text;
using System.Text.Json;

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

    protected bool TratarErrosResponse(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.BadRequest) return false;
        response.EnsureSuccessStatusCode();
        return true;
    }

}