using logiWeb.Models;
using logiWeb.Helpers; 

namespace logiWeb.Repositories
{
    public interface IOrdemRepository
    {
        AjaxResponse Cadastrar(Ordem ordem, Endereco endereco);
        AjaxResponse MostrarOrdens(int? id_funcionario, string? nome_cliente, int? status);
        AjaxResponse AlterarStatus(int id, int status);
        AjaxResponse MostrarOrdem(int id_ordem);
    }
}