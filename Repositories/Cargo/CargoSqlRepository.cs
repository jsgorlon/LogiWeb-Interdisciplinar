using System.Data.SqlClient;
using logiWeb.Models;

namespace logiWeb.Repositories
{
    public class CargoSqlRepository: DBContext, ICargoRepository
    {
        private SqlCommand cmd = new SqlCommand();

        public void Cadastrar(Cargo cargo)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"INSERT INTO Cargos (nome, descricao, salario)
                                    VALUES (@nome, @descricao, @salario)";
                
                cmd.Parameters.AddWithValue("@nome", cargo.Nome);
                cmd.Parameters.AddWithValue("@descricao", cargo.Descricao);
                cmd.Parameters.AddWithValue("@salario", cargo.Salario);

                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public List<Cargo> Mostrar()
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = "SELECT * FROM Cargos ORDER BY nome";

                SqlDataReader reader = cmd.ExecuteReader();

                List<Cargo> lista = new List<Cargo>();
                
                while (reader.Read())
                {
                    lista.Add(
                        new Cargo
                        {
                            Id = (short)reader["id"],
                            Nome = (string)reader["nome"],
                            Descricao = (string)reader["descricao"],
                            Salario = (decimal)reader["salario"]
                        }
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        
        public Cargo Mostrar(short id)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT * FROM Cargos
                                    WHERE id = @id";
                
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();
                
                if(reader.Read())
                {
                    return new Cargo{
                            Id = (short)reader["id"],
                            Nome = (string)reader["nome"],
                            Descricao = (string)reader["descricao"],
                            Salario = (decimal)reader["salario"]
                        };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public void Atualizar(short id, Cargo cargo)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE Cargos
                                SET nome = @nome, descricao = @descricao, salario = @salario
                                WHERE id = @id";
                
                cmd.Parameters.AddWithValue("@nome", cargo.Nome);
                cmd.Parameters.AddWithValue("@descricao", cargo.Descricao);
                cmd.Parameters.AddWithValue("@salario", cargo.Salario);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public void Excluir(short id)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"DELETE FROM Cargos Where id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
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