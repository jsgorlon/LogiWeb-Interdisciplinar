
namespace logiWeb.Models; 

public abstract class Pessoa {

    public int Id { get; private set; }

    public string Nome { get; set; }

    public string Email { get; set; }

    public DateTime DatCad { get; private set; } = DateTime.Now;  
}