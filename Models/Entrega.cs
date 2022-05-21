namespace logiWeb.Models
{
    public class Entrega 
    {
        public int Id { get; set; }
        public int IdFuncionario { get; set; }
        public Funcionario Funcionario {get; set;} = new Funcionario();
        public int IdMotorista { get; set; }
        public Funcionario Motorista {get; set;} = new Funcionario();
    }
}