using logiWeb.Models;

namespace logiWeb.Repositories
{
    public interface IFuncionarioRepository
    {
        bool Cadastrar(Funcionario funcionario);
        List<Funcionario> Mostrar();
        Funcionario Mostrar(int id);
        List<Funcionario> MostrarPorCargo(short id);
        List<Funcionario> MostrarPorCpf(string cpf);
        List<Funcionario> MostrarPorNome(string nome);
        void Atualizar(int id, Funcionario funcionario);
        void Excluir(int id);
    }
}