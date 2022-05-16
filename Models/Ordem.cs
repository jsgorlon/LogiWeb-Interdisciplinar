namespace logiWeb.Models
{
    public class Ordem 
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public string Destino { get; set; }
        public int Volume { get; set; }
        public decimal Peso {get; set;}
        public string Observacao { get; set; }
    }
}