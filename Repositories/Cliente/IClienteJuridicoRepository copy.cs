using logiWeb.Models;

namespace logiWeb.Repositories
{
    public interface IClienteJuridicoRepository
    {
        void CadastrarClienteJuridico(ClienteJuridico cliente);
        List<ClienteJuridico> MostrarClienteJuridico();
        ClienteJuridico MostrarClienteJuridico(int id);
        void AtualizarClienteJuridico(int id, ClienteJuridico cliente);
        void ExcluirClienteJuridico(int id);
    }
}