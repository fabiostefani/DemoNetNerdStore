namespace WebApp.MVC.Extensions
{
    public class AppSettings
    {
        public string? AutenticacaoUrl { get; set; }
        public string? CatalogoUrl { get; set; }
        public string? CarrinhoUrl { get; set; }
        public string ComprasBffUrl { get; set; } = string.Empty;
        public string ClienteUrl { get; set; } = string.Empty;
    }
}