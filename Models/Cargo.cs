
namespace logiWeb.Models;

public class Cargo 
{
  public int Id { get; }

  public string Nome {get; set;} = string.Empty;
  public string Descricao {get; set;} = string.Empty;

  public double Salario {get; set;}
}