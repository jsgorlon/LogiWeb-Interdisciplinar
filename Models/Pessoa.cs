namespace logiWeb.Models
{
    public abstract class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string? Rg { get; set; } = null;
        public DateTime DatNasc { get; set; }
        public string? Email { get; set; } = null;
        public string? Telefone { get; set; }  = null;
        public DateTime DatCad { get; set; }
    }
}