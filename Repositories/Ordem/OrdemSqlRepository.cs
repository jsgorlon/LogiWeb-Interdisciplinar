using System.Data.SqlClient;
using logiWeb.Models;

namespace logiWeb.Repositories
{
    public class OrdemSqlRepository : DBContext, IOrdemRepository
    {
        private SqlCommand cmd = new SqlCommand();

        private IClienteRepository clienteRepository;
        public OrdemSqlRepository(IClienteRepository clienteRepository)
        {
            this.clienteRepository = clienteRepository;
        }

        public void Cadastrar(Ordem ordem)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"INSERT INTO ORDENS (id_cliente, destino, volume, peso, observacao) 
                                    VALUES (@id_cliente, @destino, @volume, @peso, @observacao);";

                cmd.Parameters.AddWithValue("@id_cliente", ordem.IdCliente);
                cmd.Parameters.AddWithValue("@destino", ordem.Destino);
                cmd.Parameters.AddWithValue("@volume", ordem.Volume);
                cmd.Parameters.AddWithValue("@peso", ordem.Peso);
                cmd.Parameters.AddWithValue("@observacao", ordem.Observacao);
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

        public void Excluir(int id)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE ORDENS SET ATIVO = 0 WHERE ID_ORDEM = @ID";
                cmd.Parameters.AddWithValue("@ID", id);
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

        public List<Ordem> MostrarOrdens()
        {
            try{
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT O.ID_ORDEM, O.ID_CLIENTE, O.DESTINO, O.VOLUME, O.PESO, O.OBSERVACAO, P.NOME
                                    FROM ORDENS O
                                    INNER JOIN PESSOAS P
                                        ON O.ID_CLIENTE = P.ID 
                                    WHERE O.ATIVO > 0
                ";

                SqlDataReader reader = cmd.ExecuteReader();

                List<Ordem> lista = new List<Ordem>();

                while (reader.Read())
                {
                    lista.Add(
                        new Ordem{
                            Id = (int)reader["ID_ORDEM"],
                            Destino = (string)reader["DESTINO"],
                            Volume = (int)reader["VOLUME"],
                            Peso = (decimal)reader["PESO"],
                            Observacao = (string)reader["OBSERVACAO"],
                            //Cliente = new Cliente() (string)reader["NOME"],
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

        public Ordem MostrarOrdem(int id)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT O.ID_ORDEM, O.ID_CLIENTE, O.DESTINO, O.VOLUME, O.PESO, O.OBSERVACAO, P.NOME
                                    FROM ORDENS O
                                    INNER JOIN PESSOAS P
                                        ON O.ID_CLIENTE = P.ID 
                                    WHERE ID_ORDEM = @ID
                                    AND  O.ATIVO > 0";

                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();

                SqlDataReader reader = cmd.ExecuteReader();

                Ordem ordem = new Ordem();

                while (reader.Read())
                {
                        
                    ordem.Id = (int)reader["ID_ORDEM"];
                    ordem.Destino = (string)reader["DESTINO"];
                    ordem.Volume = (int)reader["VOLUME"];
                    ordem.Peso = (decimal)reader["PESO"];
                    ordem.Observacao = (string)reader["OBSERVACAO"];
                    ordem.Cliente.Nome = (string)reader["NOME"];
                }
                return ordem;
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