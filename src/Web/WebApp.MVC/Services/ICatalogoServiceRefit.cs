using Refit;
using WebApp.MVC.Models;

namespace WebApp.MVC.Services;

public interface ICatalogoServiceRefit
{
    [Get("/catalogo/produtos")]
    Task<IEnumerable<ProdutoViewModel>> ObterTodos();
    
    [Get("/catalogo/produtos/{id}")]
    Task<ProdutoViewModel> ObterPorId(Guid id);
}