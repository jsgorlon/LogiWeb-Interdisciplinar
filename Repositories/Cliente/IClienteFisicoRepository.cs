using logiWeb.Models;

namespace logiWeb.Repositories
{
    public interface IClienteFisicoRepository
    {
        void CadastrarClienteFisico(ClienteFisico cliente);
        List<ClienteFisico> MostrarClienteFisico();
        ClienteFisico MostrarClienteFisico(int id);
        void AtualizarClienteFisico(int id, ClienteFisico cliente);
        void ExcluirClienteFisico(int id);
    }
}