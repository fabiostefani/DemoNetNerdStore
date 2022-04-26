using Microsoft.AspNetCore.Mvc;
using WebApp.MVC.Models;
using WebApp.MVC.Services;

namespace WebApp.MVC.Controllers
{
    public class CatalogoController : MainController
    {
        private readonly ICatalogoServiceRefit _catalogoService;

        public CatalogoController(ICatalogoServiceRefit catalogoService)
        {
            _catalogoService = catalogoService;
        }
        
        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProdutoViewModel>? produtos = await _catalogoService.ObterTodos();
            return View(produtos);
        }

        [HttpGet]
        [Route("produto-detalhe/{id:guid}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            ProdutoViewModel? produto = await _catalogoService.ObterPorId(id);
            return View(produto);
        }
    }
}