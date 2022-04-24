using logiWeb.Models; 

namespace logiWeb.Models; 

public abstract class PessoaJuridica : Pessoa 
{
  public string Cnpj {get; set;} = string.Empty;

  public string RazaoSocial {get; set;} = string.Empty;
}