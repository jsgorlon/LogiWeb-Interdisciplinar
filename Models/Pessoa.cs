
namespace logiWeb.Models; 

public abstract class Pessoa {

    public int Id { get; private set; } = (int)DateTimeOffset.Now.ToUnixTimeMilliseconds(); //#TODO Remover quando não for mais usado para teste sem o banco de dados

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime DatCad { get; private set; } = DateTime.Now;  
}