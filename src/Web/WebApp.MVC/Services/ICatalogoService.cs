using WebApp.MVC.Models;

namespace WebApp.MVC.Services;

public interface ICatalogoService
{
    Task<IEnumerable<ProdutoViewModel>?> ObterTodos();
    Task<ProdutoViewModel?> ObterPorId(Guid id);
}