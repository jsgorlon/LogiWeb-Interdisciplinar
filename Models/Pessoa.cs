
namespace logiWeb.Models; 

public abstract class Pessoa {

    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime DatCad { get; set; }  
}