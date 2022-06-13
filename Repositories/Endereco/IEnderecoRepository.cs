using logiWeb.Models;

namespace logiWeb.Repositories
{
    public interface IEnderecoRepository
    {
        void Cadastrar(Endereco endereco);
        List<Endereco> MostrarEstado();
        List<Endereco> MostrarCidade();
        Endereco MostrarEndereco(string cep, string nrCasa);
    }
}