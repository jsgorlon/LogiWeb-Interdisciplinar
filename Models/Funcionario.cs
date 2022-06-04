namespace logiWeb.Models
{
  public class Funcionario : Pessoa
  {
    public short IdCargo { get; set; }
    public Cargo? Cargo { get; set; } = new Cargo();
    public string Login { get; set; }
    public string Senha { get; set; }
    public bool Ativo { get; set;} = true; 
  }
}