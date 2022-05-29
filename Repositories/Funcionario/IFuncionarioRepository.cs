using logiWeb.Models;

namespace logiWeb.Repositories
{
    public interface IFuncionarioRepository
    {
        void Cadastrar(Funcionario funcionario);
        List<Funcionario> Mostrar();
        Funcionario Mostrar(int id);
        List<Funcionario> MostrarPorCargo(short id);
        void Atualizar(int id, Funcionario funcionario);
        void Excluir(int id);
    }
}