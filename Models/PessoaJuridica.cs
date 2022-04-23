using logiWeb.Models; 

namespace logiWeb.Models; 

public abstract class PessoaJuridica : Pessoa 
{
  public string cnpj {get; set;}

  public string RazaoSocial {get; set;}
}