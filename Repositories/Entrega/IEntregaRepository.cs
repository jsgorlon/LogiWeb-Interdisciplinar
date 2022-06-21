using logiWeb.Models;
using logiWeb.Helpers; 
namespace logiWeb.Repositories
{
    public interface IEntregaRepository
    {
        AjaxResponse Cadastrar(int id_funcionario, int id_motorista);
        AjaxResponse MostrarEntregas(int? id_funcionario, int? id_motorista);
        AjaxResponse AdicionarOrdem(int id_entrega, int id_ordem); 
        AjaxResponse MostrarEntregaOrdem(int id_entrega); 
        Entrega MostrarEntrega(int id);
        void Excluir(int id);
        void StatusOrdem(Ordem ordem);
        void StatusEntrega(Entrega entrega);
        Entrega MostrarDetalheEntrega(int id);
        
    }
}