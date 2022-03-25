using Core.DomainObjects;

namespace Catalogo.API.Models
{
    public class Produto : Entity, IAggregateRoot
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Imagem { get; set; }
        public int QuantidadeEstoque { get; set; }
        public Produto(string nome, string descricao, string imagem)
        {
            Nome = nome;
            Descricao = descricao;
            Imagem = imagem;
        }

        protected Produto() : this(string.Empty, string.Empty, string.Empty)
        {
            
        }
    }
}