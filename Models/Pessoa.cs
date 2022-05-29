namespace logiWeb.Models
{
    public abstract class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Rg { get; set; } = string.Empty;
        public DateTime DatNasc { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public DateTime DatCad { get; set; }
    }
}