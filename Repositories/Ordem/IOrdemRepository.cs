using logiWeb.Models;

namespace logiWeb.Repositories
{
    public interface IOrdemRepository
    {
        string Cadastrar(Ordem ordem);
        List<Ordem> MostrarOrdens();
        Ordem MostrarOrdem(int id);
        void Excluir(int id);
    }
}