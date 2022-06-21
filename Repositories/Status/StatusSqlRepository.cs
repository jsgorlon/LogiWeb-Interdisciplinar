using System.Data.SqlClient;
using logiWeb.Models;
using logiWeb.Helpers; 
namespace logiWeb.Repositories
{
    public class StatusSqlRepository : DBContext, IStatusRepository
    {
       
        private AjaxResponse response = new AjaxResponse(); 
        public AjaxResponse Mostrar()
        {
            try{
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT ID, NOME, DESCRICAO FROM STATUS where descricao like @obj ";
                cmd.Parameters.AddWithValue("@obj", "%Objeto%");
                SqlDataReader reader = cmd.ExecuteReader();

                List<Entrega> lista = new List<Entrega>();

                while (reader.Read())
                {
                   Entrega item = new Entrega();
                   item.Status.Id = (short)reader["id"];
                   item.Status.Nome = (string)reader["NOME"];
                   item.Status.Descricao = (string)reader["DESCRICAO"];
                   lista.Add(item);
                }
                response.Item.Add("entregas", lista); 
                return response;
            }catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
    }
}