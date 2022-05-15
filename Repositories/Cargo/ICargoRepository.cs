using logiWeb.Models;

namespace logiWeb.Repositories
{
    public interface ICargoRepository
    {
        void Cadastrar(Cargo cargo);
        Cargo Mostrar(int id);
        List<Cargo> Mostrar();
        void Atualizar(int id, Cargo cargo);
        void Excluir(int id);
    }
}