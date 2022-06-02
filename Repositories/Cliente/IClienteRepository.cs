using logiWeb.Models;

namespace logiWeb.Repositories
{
    public interface IClienteRepository
    {
        void Cadastrar(Cliente cliente);
        List<Cliente> Mostrar();
        Cliente Mostrar(int id);
        List<Cliente> MostrarPorCpf(string cpf);
        List<Cliente> MostrarPorNome(string nome);
        void Atualizar(int id, Cliente cliente);
        void Excluir(int id);
    }
}