using logiWeb.Models;

namespace logiWeb.Repositories
{
    public class ClienteRepository
    {
        private static List<ClienteFisico> clientes_fisicos = new List<ClienteFisico>();
        private static List<ClienteJuridico> clientes_juridicos = new List<ClienteJuridico>();

        public List<ClienteFisico> MostrarClientesFisicos()
        {
            return clientes_fisicos;
        }

        public void CadastrarClienteFisico(ClienteFisico cliente)
        {
            clientes_fisicos.Add(cliente);
        }

        public void CadastrarClienteJuridico(ClienteJuridico cliente)
        {
            clientes_juridicos.Add(cliente);
        }

        public List<ClienteJuridico> MostrarClientesJuridicos()
        {
            return clientes_juridicos;
        }
    }
}