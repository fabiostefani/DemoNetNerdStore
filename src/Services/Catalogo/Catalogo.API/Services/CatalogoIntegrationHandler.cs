using Catalogo.API.Models;
using Core.DomainObjects;
using Core.Message.Integration;
using MessageBus;

namespace Catalogo.API.Services;

public class CatalogoIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public CatalogoIntegrationHandler(IMessageBus bus, 
                                      IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetSubscribers();
        return Task.CompletedTask;
    }

    private void SetSubscribers()
    {
        _bus.SubscribeAsync<PedidoAutorizadoIntegrationEvent>("PedidoAutorizado", async request =>
            await BaixarEstoque(request));
    }

    private async Task BaixarEstoque(PedidoAutorizadoIntegrationEvent message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var produtosRepository = scope.ServiceProvider.GetRequiredService<IProdutoRepository>();
            var idsProdutos = string.Join(",", message.Itens.Select(c => c.Key));
            var produtos = await produtosRepository.ObterProdutosPorId(idsProdutos);
            if (produtos.Count != message.Itens.Count)
            {
                CancelarPedidoSemEstoque(message);
                return;
            }
            var produtosComEstoque = new List<Produto>();
            foreach (var produto in produtos)
            {
                var quantidadeProduto = message.Itens.FirstOrDefault(p => p.Key == produto.Id).Value;
                if (produto.EstaDisponivel(quantidadeProduto))
                {
                    produto.RetirarEstoque(quantidadeProduto);
                    produtosComEstoque.Add((produto));
                }
            }

            if (produtosComEstoque.Count != message.Itens.Count)
            {
                CancelarPedidoSemEstoque(message);
                return;
            }

            foreach (var produto in produtosComEstoque)
            {
                produtosRepository.Atualizar(produto);
            }

            if (! await produtosRepository.UnitOfWork.Commit())
            {
                throw new DomainException($"Problemas ai atualizar estoque do pedido {message.PedidoId}");
            }

            var pedidoBaixadoEvent = new PedidoBaixadoIntegrationEvent(message.ClienteId, message.PedidoId);
            await _bus.PublishAsync(pedidoBaixadoEvent);

        }
    }

    private async void CancelarPedidoSemEstoque(PedidoAutorizadoIntegrationEvent message)
    {
        var pedidoCancelado = new PedidoCanceladoIntegrationEvent(message.ClienteId, message.PedidoId);
        await _bus.PublishAsync(pedidoCancelado);
    }
}