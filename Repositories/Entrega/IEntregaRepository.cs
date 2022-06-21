using logiWeb.Models;
using logiWeb.Helpers; 
namespace logiWeb.Repositories
{
    public interface IEntregaRepository
    {
        AjaxResponse Cadastrar(int IdFuncionario, int idMotorista, int[] idOrdem);
        AjaxResponse MostrarEntregas(int? id_funcionario, int? id_motorista);
        List<Ordem> MostrarOrdensEntrega(int id);
        AjaxResponse Excluir(int id);
        AjaxResponse StatusOrdem(int id_ordem, int id_status, int id_entrega);
        void StatusEntrega(int id_entrega, int id_status);
        AjaxResponse MostrarDetalheEntrega(int id);
    }
}