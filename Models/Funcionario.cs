using logiWeb.Models; 
namespace logiWeb.Models; 

public abstract class Funcionario : PessoaFisica
{
  public int IdCargo { get; set; }
  public double BonusExtra { get; set; } = 0; 

  public string Login { get; set; } = string.Empty;
  public string Senha { get; set; } = string.Empty;
  public bool Ativo { get; set;}
}   