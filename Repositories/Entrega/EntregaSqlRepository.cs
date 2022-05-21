using System.Data.SqlClient;
using logiWeb.Models;

namespace logiWeb.Repositories
{
    public class EntregaSqlRepository : DBContext, IEntregaRepository
    {
        private SqlCommand cmd = new SqlCommand();
        private IOrdemRepository ordemRepository;
        private IStatusRepository statusRepository;

        public void Cadastrar(Entrega entrega)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"INSERT INTO ENTREGAS (id_ordem, id_funcionario, id_motorista) 
                                    VALUES (@id_ordem, @id_funcionario, @id_motorista)";

                cmd.Parameters.AddWithValue("@id_ordem", ordem.IdOrdem);
                cmd.Parameters.AddWithValue("@id_funcionario", ordem.IdFuncionario);
                cmd.Parameters.AddWithValue("@id_motorista", ordem.IdMotorista);
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
                cmd.CommandText = @"UPDATE ENTREGAS SET ATIVO = 0 WHERE ID_ENTREGA = @ID";
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

        public List<Ordem> MostrarEntregas()
        {
            try{
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT E.ID, E.ID_ORDEM, E.ID_FUNCIONARIO, E.ID_MOTORISTA, 
                                    F.NOME_FUNCIONARIO, P.NOME_MOTORISTA,
                                    O.DESTINO, O.VOLUME, O.PESO, O.OBSERVACAO, P.NOME
                                    FROM ORDENS O
                                        ON E.ID_ORDEM = O.ID
                                    INNER JOIN ORDENS F
                                        ON E.ID_FUNCIONARIO = F.ID 
                                    INNER JOIN PESSOAS F
                                        ON E.ID_FUNCIONARIO = F.ID 
                                    INNER JOIN PESSOAS M
                                        ON E.ID_MOTORISTA = M.ID 
                                    INNER JOIN PESSOAS P
                                        ON O.ID_CLIENTE = P.ID
                                    WHERE E.ATIVO > 0
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
                            NomeCliente = (string)reader["NOME"],
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

        public Ordem MostrarEntrega(int id)
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
                    ordem.NomeCliente = (string)reader["NOME"];
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