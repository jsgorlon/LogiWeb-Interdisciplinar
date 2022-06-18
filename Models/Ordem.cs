namespace logiWeb.Models
{
    public class Ordem 
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdFuncionario { get; set; }
        public int Qtd_itens { get; set; }
        public Funcionario Funcionario {get; set;} = new Funcionario();
        public Cliente Cliente {get; set;} = new Cliente();
        public string Volume { get; set; } = string.Empty;
        public decimal Peso {get; set;}
        public string Observacao { get; set; } = string.Empty;
        public int IdStatus { get; set; }
        public bool Ativo { get; set; }
        public int IdEndereco { get; set; }
        public Status Status {get; set;} = new Status();
        public Endereco Endereco {get; set;} = new Endereco();
    }
}