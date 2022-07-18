namespace Identidade.API.Models;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public Guid Token { get; set; }
    public DateTime DataExpiracao { get; set; }

    public RefreshToken()
    {
        Id = Guid.NewGuid();
        Token = Guid.NewGuid();
    }
}