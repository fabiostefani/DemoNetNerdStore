using Core.DomainObjects;

namespace Pagamentos.API.Models;

public class Transacao : Entity
{
    public string CodigoAutorizacao { get; set; } = string.Empty;
    public string BandeiraCartao { get; set; }= string.Empty;
    public DateTime? DataTransacao { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal CustoTransacao { get; set; }
    public StatusTransacao Status { get; set; }
    public string TID { get; set; } = string.Empty;// Id
    public string NSU { get; set; } = string.Empty;// Meio (paypal)

    public Guid PagamentoId { get; set; }

    // EF Relation
    public Pagamento? Pagamento { get; set; }
}