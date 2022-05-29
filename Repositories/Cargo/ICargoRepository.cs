using logiWeb.Models;

namespace logiWeb.Repositories
{
    public interface ICargoRepository
    {
        void Cadastrar(Cargo cargo);
        Cargo Mostrar(short id);
        List<Cargo> Mostrar();
        void Atualizar(short id, Cargo cargo);
        void Excluir(short id);
    }
}