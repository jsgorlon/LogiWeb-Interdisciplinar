using logiWeb.Models;

namespace logiWeb.Repositories
{
    public interface IEntregaRepository
    {
        void Cadastrar(Entrega entrega, int[] id);
        List<Entrega> MostrarEntregas();
        Entrega MostrarEntrega(int id);
        void Excluir(int id);
        void StatusOrdem(Ordem ordem);
        void StatusEntrega(Entrega entrega);
    }
}