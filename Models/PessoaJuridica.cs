using logiWeb.Models; 

namespace logiWeb.Models; 

abstract class PessoaJuridica : Pessoa 
{
  public string cnpj {get; set;}

  public string RazaoSocial {get; set;}
}