using Dapper;
using Pedidos.API.Application.Dtos;
using Pedidos.Domain.Pedidos;

namespace Pedidos.API.Application.Queries;

public class PedidoQueries : IPedidoQueries
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoQueries(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }
    public async Task<PedidoDto> ObterUltimoPedido(Guid clienteId)
    {
        // const string sql = @"SELECT
        //                         P.ID AS 'ProdutoId', P.CODIGO, P.VOUCHERUTILIZADO, P.DESCONTO, P.VALORTOTAL,P.PEDIDOSTATUS,
        //                         P.LOGRADOURO,P.NUMERO, P.BAIRRO, P.CEP, P.COMPLEMENTO, P.CIDADE, P.ESTADO,
        //                         PIT.ID AS 'ProdutoItemId',PIT.PRODUTONOME, PIT.QUANTIDADE, PIT.PRODUTOIMAGEM, PIT.VALORUNITARIO 
        //                         FROM PEDIDOS P 
        //                         INNER JOIN PEDIDOITEMS PIT ON P.ID = PIT.PEDIDOID 
        //                         WHERE P.CLIENTEID = @clienteId 
        //                         AND P.DATACADASTRO between DATEADD(minute, -3,  GETDATE()) and DATEADD(minute, 0,  GETDATE())
        //                         AND P.PEDIDOSTATUS = 1 
        //                         ORDER BY P.DATACADASTRO DESC";
        const string sql = @"SELECT
                                p.'Id' AS 'ProdutoId', p.'Codigo' , p.'VoucherUtilizado' , p.'Desconto' , p.'ValorTotal' ,p.'PedidoStatus' ,
                                p.'Logradouro' ,p.'Numero' , p.'Bairro' , p.'Cep' , p.'Complemento' , p.'Cidade' , p.'Estado' ,
                                pit.'Id' AS 'ProdutoItemId',pit.'ProdutoNome' , pit.'Quantidade' , pit.'ProdutoImagem' , pit.'ValorUnitario'  
                                FROM 'Pedidos' p  
                                    INNER JOIN 'PedidoItems' pit  ON p.'Id'  = pit.'PedidoId' 
                                WHERE p.'ClienteId'  = @clienteId                                
                                AND p.'PedidoStatus'  = 1   
                                and p.'DataCadastro' between now() + '3 minutes' and now()
                                ORDER BY p.'DataCadastro'  DESC";
        var pedido = await _pedidoRepository.ObterConexao()
            .QueryAsync<dynamic>(sql, new {clienteId});
        return MapearPedido(pedido);
    }

    public async Task<IEnumerable<PedidoDto>> ObterListaPorClienteId(Guid clienteId)
    {
        var pedidos = await _pedidoRepository.ObterListaPorClienteId(clienteId);
        return pedidos.Select(PedidoDto.ParaPedidoDto);
    }

    public async Task<PedidoDto?> ObterPedidosAutorizados()
    {
        const string sql = @"select p.""Id"" as ""PedidoId"",
                                    p.""Id"",
                                    p.""ClienteId"",
                                    pi2.""Id"" as ""PedidoItemId"",
                                    pi2.""Id"",
                                    pi2.""ProdutoId"",
                                    pi2.""Quantidade""
                                    from ""Pedidos"" p 
                                        inner join ""PedidoItems"" pi2 on p.""Id"" = pi2.""PedidoId"" 
                                    where p.""PedidoStatus"" = 1
                                    order by p.""DataCadastro"" 
                                    limit 1";
        IEnumerable<PedidoDto?> pedido = await _pedidoRepository.ObterConexao().QueryAsync<PedidoDto, PedidoItemDto, PedidoDto>(sql,
            (p, pi) =>
            {
                p.PedidoItens = new List<PedidoItemDto>();
                p.PedidoItens.Add(pi);
                return p;
            }, splitOn: "PedidoId,PedidoItemId");
        return pedido.FirstOrDefault();
    }

    private PedidoDto MapearPedido(dynamic result)
    {
        var pedido = new PedidoDto
        {
            Codigo = result[0].CODIGO,
            Status = result[0].PEDIDOSTATUS,
            ValorTotal = result[0].VALORTOTAL,
            Desconto = result[0].DESCONTO,
            VoucherUtilizado = result[0].VOUCHERUTILIZADO,

            PedidoItens = new List<PedidoItemDto>(),
            Endereco = new EnderecoDto
            {
                Logradouro = result[0].LOGRADOURO,
                Bairro = result[0].BAIRRO,
                Cep = result[0].CEP,
                Cidade = result[0].CIDADE,
                Complemento = result[0].COMPLEMENTO,
                Estado = result[0].ESTADO,
                Numero = result[0].NUMERO
            }
        };

        foreach (var item in result)
        {
            var pedidoItem = new PedidoItemDto()
            {
                Nome = item.PRODUTONOME,
                Valor = item.VALORUNITARIO,
                Quantidade = item.QUANTIDADE,
                Imagem = item.PRODUTOIMAGEM
            };

            pedido.PedidoItens.Add(pedidoItem);
        }

        return pedido;  
    } 
}