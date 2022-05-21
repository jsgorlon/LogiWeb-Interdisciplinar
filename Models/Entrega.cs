namespace logiWeb.Models
{
    public class Entrega 
    {
        public int Id { get; set; }     
        public int IdOrdem { get; set; }
        public Ordem Ordem {get; set;} = new Ordem();
        public int IdFuncionario { get; set; }
        public Funcionario Funcionario {get; set;} = new Funcionario();
        public int IdMotorista { get; set; }
        public Funcionario Motorista {get; set;} = new Funcionario();
    }
}