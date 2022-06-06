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
            .QueryAsync<PedidoDto, PedidoItemDto, EnderecoDto, PedidoDto>(sql, (p, pi, e) =>
            {
               p.PedidoItens.Add(pi);
               p.Endereco = e;
               return p;
            }, new {clienteId});
        return MapearPedido();
    }

    public async Task<IEnumerable<PedidoDto>> ObterListaPorClienteId(Guid clienteId)
    {
        var pedidos = await _pedidoRepository.ObterListaPorClienteId(clienteId);
        return pedidos.Select(PedidoDto.ParaPedidoDto);
    }

    private PedidoDto MapearPedido()
        => new PedidoDto();
}