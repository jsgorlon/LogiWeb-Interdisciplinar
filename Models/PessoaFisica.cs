using logiWeb.Models; 

namespace logiWeb.Models;

public abstract class PessoaFisica : Pessoa 
{

    public string Cpf { get; set; } = string.Empty;

    public string Rg { get; set; } = string.Empty;

    public DateOnly DatNasc { get; set; } 
}