using logiWeb.Models;
using logiWeb.Helpers; 

namespace logiWeb.Repositories
{
    public interface IOrdemRepository
    {
        AjaxResponse Cadastrar(Ordem ordem, Endereco endereco);
        List<Ordem> MostrarOrdens();
        Ordem MostrarOrdem(int id);
        void Excluir(int id);
    }
}