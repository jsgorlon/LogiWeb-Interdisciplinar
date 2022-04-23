using logiWeb.Models; 
namespace logiWeb.Models; 

public abstract class Funcionario : PessoaFisica 
{
  public double BonusExtra { get; set; } = 0; 

  public string Login { get; set; }
  public string Senha { get; set; }  
  public bool Ativo { get; set;}
}   