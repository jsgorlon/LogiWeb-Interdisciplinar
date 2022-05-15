namespace logiWeb.Models
{
  public class Cargo 
  {
    public int Id { get; set;}
    public string Nome {get; set;} = string.Empty;
    public string Descricao {get; set;} = string.Empty;
    public decimal Salario {get; set;}
  }
} 