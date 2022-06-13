using logiWeb.Models;
using logiWeb.Helpers; 

namespace logiWeb.Repositories
{
    public interface IClienteRepository
    {
        AjaxResponse Cadastrar(Cliente cliente);
        AjaxResponse Mostrar(string? nome, int? status);
        AjaxResponse Atualizar(int id, Cliente cliente);
        AjaxResponse AlterarStatus(int id, int status);
    }
}