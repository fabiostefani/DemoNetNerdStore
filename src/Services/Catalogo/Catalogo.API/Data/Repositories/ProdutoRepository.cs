using Catalogo.API.Models;
using Core.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.API.Data.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CatalogoContext _context;
        public IUnitOfWork UnitOfWork => _context;
        
        public ProdutoRepository(CatalogoContext context)
        {
            _context = context;

        }
        public async Task<PagedResult<Produto>> ObterTodos(int pageSize, int pageIndex, string? query = null)
        {
            var sql = @$"select * 
                          from produtos p    
                        where (p.""Nome"" like '%' || @nome || '%' or @nome is null)
                         order by p.""Nome""                                                
                         limit {pageSize}
                         OFFSET {pageSize * (pageIndex - 1)};
                    select count(p.""Id"") from produtos p
                     where (p.""Nome"" like '%' || @nome || '%' or @nome is null)";
            
            var multi = await _context.Database.GetDbConnection()
                .QueryMultipleAsync(sql, new {nome = query});
            
            var produtos = multi.Read<Produto>();
            var total = multi.Read<int>().FirstOrDefault();

            return new PagedResult<Produto>()
            {
                List = produtos,
                TotalResults = total,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query
            };

            // return await _context.Produtos.AsNoTracking()
            //     .Skip(pageSize * (pageIndex - 1))
            //     .Take(pageSize)
            //     .ToListAsync();
        }

        public async Task<Produto>? ObterPorId(Guid id)
        {
            return await _context.Produtos.FindAsync(id);
        }
        
        public async Task<List<Produto>> ObterProdutosPorId(string ids)
        {
            var idsGuid = ids.Split(',')
                .Select(id => (Ok: Guid.TryParse(id, out var x), Value: x));

            if (!idsGuid.All(nid => nid.Ok)) return new List<Produto>();

            var idsValue = idsGuid.Select(id => id.Value);

            return await _context.Produtos.AsNoTracking()
                .Where(p => idsValue.Contains(p.Id) && p.Ativo).ToListAsync();
        }

        public void Adicionar(Produto produto)
        {
            _context.Produtos.Add(produto);
        }

        public void Atualizar(Produto produto)
        {
            _context.Produtos.Update(produto);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}