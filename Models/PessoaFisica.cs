using logiWeb.Models; 

namespace logiWeb.Models;

public abstract class PessoaFisica : Pessoa 
{

    public string Cpf { get; set; }

    public string Rg { get; set; }

    public DateOnly DataNasc { get; set; } 
}