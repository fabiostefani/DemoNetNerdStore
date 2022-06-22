using Core.Communication;
using WebApp.MVC.Models;

namespace WebApp.MVC.Services;

public interface IClienteService
{
    Task<EnderecoViewModel?> ObterEndereco();
    Task<ResponseResult?> AdicionarEndereco(EnderecoViewModel endereco);
}