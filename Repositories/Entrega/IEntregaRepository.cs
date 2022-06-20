using logiWeb.Models;
using logiWeb.Helpers; 
namespace logiWeb.Repositories
{
    public interface IEntregaRepository
    {
        void Cadastrar(Entrega entrega, int[] id);
        AjaxResponse MostrarEntregas(int? id_funcionario, int? id_motorista);
        Entrega MostrarEntrega(int id);
        void Excluir(int id);
        void StatusOrdem(Ordem ordem);
        void StatusEntrega(Entrega entrega);
        Entrega MostrarDetalheEntrega(int id);
    }
}