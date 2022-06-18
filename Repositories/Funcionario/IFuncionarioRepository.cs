using logiWeb.Models;
using logiWeb.Helpers;

namespace logiWeb.Repositories
{
    public interface IFuncionarioRepository
    {
        string Cadastrar(Funcionario funcionario);
        AjaxResponse Mostrar(string? nome, int? id_cargo, int? status);

        string Atualizar(int id, Funcionario funcionario);
        void AlterarStatus(int id, int status);
    }
}