using System.Reflection.Metadata.Ecma335;

namespace Pagamentos.NerdsPag;

public class NerdsPagService
{
    public readonly string ApiKey;
    public readonly string EncryptionKey;

    public NerdsPagService(string apiKey, string encryptionKey)
    {
        ApiKey = apiKey;
        EncryptionKey = encryptionKey;
    }
}