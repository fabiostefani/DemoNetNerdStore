using WebApp.MVC.Models;

namespace WebApp.MVC.Services;

public interface ICatalogoService
{
    Task<PagedViewModel<ProdutoViewModel>?> ObterTodos(int pageSize, int pageIndex, string? query = null);
    Task<ProdutoViewModel?> ObterPorId(Guid id);
}