using logiWeb.Models;

namespace logiWeb.Repositories
{
    public interface IOrdemRepository
    {
        void Cadastrar(Ordem ordem);
        List<Ordem> MostrarOrdens();
        Ordem MostrarOrdem(int id);
        void Excluir(int id);
    }
}