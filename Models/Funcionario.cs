namespace logiWeb.Models
{
  public class Funcionario : Pessoa
  {
    public int IdCargo { get; set; }
    public Cargo Cargo { get; set; } = new Cargo();
    public string Login { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public bool Ativo { get; set;}
  }
}