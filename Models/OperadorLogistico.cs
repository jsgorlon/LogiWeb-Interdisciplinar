
using logiWeb.Models;

namespace logiWeb.Models;

public class OperadorLogistico : Funcionario 
{
  // [Params Motorista motorista, Ordem ordem]
   public bool CriarEntrega() 
   {

     return true; 
   }

   public bool RemoverEntrega(Entrega entrega)
   {
     return true; 
   }

   
}