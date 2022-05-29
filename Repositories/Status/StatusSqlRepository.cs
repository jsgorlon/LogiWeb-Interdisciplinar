using System.Data.SqlClient;
using logiWeb.Models;

namespace logiWeb.Repositories
{
    public class StatusSqlRepository : DBContext, IStatusRepository
    {
        private SqlCommand cmd = new SqlCommand();

        public List<Status> Mostrar()
        {
            try{
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT ID, NOME, DESCRICAO FROM STATUS ";

                SqlDataReader reader = cmd.ExecuteReader();

                List<Status> lista = new List<Status>();

                while (reader.Read())
                {
                    lista.Add(
                        new Status{
                            Id = (short)reader["ID_ORDEM"],
                            Nome = (string)reader["NOME"],
                            Descricao = (string)reader["DESCRICAO"]
                        }
                    );
                }
                return lista;
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