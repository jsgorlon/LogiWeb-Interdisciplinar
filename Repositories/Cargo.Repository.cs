using logiWeb.Models;
namespace logiWeb.Repositories
{
    public class CargoRepository
    {
        private List<Cargo> cargos = new List<Cargo>();
        public CargoRepository(){
            Cargo c1 = new Cargo();
            c1.Id = 1;
            c1.Nome = "Motorista";
            c1.Descricao = "Transportar cargas";

            Cargo c2 = new Cargo();
            c2.Id = 2;
            c2.Nome = "Atendente";
            c2.Descricao = "Atender clientes e realizar abertura de ordens de serviço";

            cargos.Add(c1);
            cargos.Add(c2);            
        }
        public List<Cargo> Read()
            {
                return cargos;
            }
    }
}