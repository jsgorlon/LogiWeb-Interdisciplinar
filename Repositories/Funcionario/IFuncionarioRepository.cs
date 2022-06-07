using logiWeb.Models;

namespace logiWeb.Repositories
{
    public interface IFuncionarioRepository
    {
        string Cadastrar(Funcionario funcionario);
        List<Funcionario> Mostrar(string? nome, int? id_cargo, int? status);
        Funcionario Mostrar(int id);
        List<Funcionario> MostrarPorCargo(short id);
        List<Funcionario> MostrarPorCpf(string cpf);
        List<Funcionario> MostrarPorNome(string nome);
        string Atualizar(int id, Funcionario funcionario);
        void AlterarStatus(int id, int status);
    }
}