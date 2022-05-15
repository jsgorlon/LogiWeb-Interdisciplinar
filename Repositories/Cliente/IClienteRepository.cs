using logiWeb.Models;

namespace logiWeb.Repositories
{
    public interface IClienteRepository
    {
        void Cadastrar(Cliente cliente);
        List<Cliente> Mostrar();
        Cliente Mostrar(int id);
        Cliente MostrarPorCpf(string cpf);
        void Atualizar(int id, Cliente cliente);
        void Excluir(int id);
    }
}