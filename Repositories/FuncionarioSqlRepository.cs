using logiWeb.Models;

namespace logiWeb.Repositories
{
    public class FuncionarioSqlRepository : DBContext, IFuncionarioRepository
    {
        public void Cadastrar(Funcionario funcionario)
        {
            throw new NotImplementedException();
        }
        public void Excluir(int id)
        {
            throw new NotImplementedException();
        }
        public List<Funcionario> Mostrar()
        {
            throw new NotImplementedException();
        }
        public void Atualizar(int id, Funcionario funcionario) 
        {
            throw new NotImplementedException();
        }
    }
}