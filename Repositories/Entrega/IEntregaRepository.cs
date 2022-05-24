using logiWeb.Models;

namespace logiWeb.Repositories
{
    public interface IEntregaRepository
    {
        void Cadastrar(Entrega entrega);
        List<Entrega> MostrarEntregas();
        Entrega MostrarEntrega(int id);
        void Excluir(int id);

    }
}