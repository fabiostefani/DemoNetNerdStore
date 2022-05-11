using Microsoft.Extensions.Configuration;

namespace Core.Utils;

public static class ConfigurationExtensions
{
    private const string NomeSecao = "MessageQueueConnection";
    public const string NomeConexao = "MessageBus";
    //sumary
    public static string GetMessageQueueConnection(this IConfiguration configuration, string name)
    {
        var section = configuration?.GetSection(NomeSecao);
        if (section == null) throw new Exception("Configuração do MessageQueue não localizada.");
        return section[name];
    }
}