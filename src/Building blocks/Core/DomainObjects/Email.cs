using System.Text.RegularExpressions;

namespace Core.DomainObjects;

public class Email
{
    public const int EnderecoMaxLength = 254;
    public const int EnderecoMinLength = 5;
    public string Endereco { get; private set; }

    protected Email()
    { }

    public Email(string email)
    {
        if (!Validar(email)) throw new DomainException("E-mail inválido.");
        Endereco = email;
    }

    public static bool Validar(string email)
    {
        var regexEmail = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        return regexEmail.IsMatch(email);
    }
}