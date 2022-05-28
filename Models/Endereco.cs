namespace logiWeb.Models
{
    public class Endereco 
    {
        public int Id { get; set; }
        public int IdCidade { get; set; }
        public string Cidade { get; set; } = string.Empty;
        public int IdEstado { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Nr_casa { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string Bairro {get; set;}  = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        
    }
}