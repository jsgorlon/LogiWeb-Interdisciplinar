using logiWeb.Models;
namespace logiWeb.Repositories;

public class FuncionarioRepository
{
    private List<Funcionario> Funcionario = new List<Funcionario>();
    
    public FuncionarioRepository()
    {
        Funcionario f1 = new Funcionario();
       // f1.Nome = "Luis";

    }
}