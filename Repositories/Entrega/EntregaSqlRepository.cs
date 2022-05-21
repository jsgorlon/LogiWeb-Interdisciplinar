using System.Data.SqlClient;
using logiWeb.Models;

namespace logiWeb.Repositories
{
    public class EntregaSqlRepository : DBContext, IEntregaRepository
    {
        private SqlCommand cmd = new SqlCommand();
        private IOrdemRepository ordemRepository;
        private IStatusRepository statusRepository;

        public EntregaSqlRepository(IOrdemRepository ordemRepository,IStatusRepository statusRepository)
        {
            this.ordemRepository = ordemRepository;
            this.statusRepository = statusRepository;
        }

        public void Cadastrar(Entrega entrega)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"INSERT INTO ENTREGAS (id_ordem, id_funcionario, id_motorista) 
                                    VALUES (@id_ordem, @id_funcionario, @id_motorista); ";

                cmd.Parameters.AddWithValue("@id_ordem", entrega.IdOrdem);
                cmd.Parameters.AddWithValue("@id_funcionario", entrega.IdFuncionario);
                cmd.Parameters.AddWithValue("@id_motorista", entrega.IdMotorista);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "select SCOPE_IDENTITY() as id";
                SqlDataReader reader = cmd.ExecuteReader();
                int idEntrega;

                while (reader.Read())
                {         
                    idEntrega = (int)reader["ID"];
                }
                //status padrao 1
                 cmd.CommandText = @"INSERT INTO entregas_ordens (id_ordem, id_entrega, id_status) 
                                    VALUES (@id_ordem, @id_entrega, 1); ";

                cmd.Parameters.AddWithValue("@id_ordem", entrega.IdOrdem);
               // cmd.Parameters.AddWithValue("@id_entrega",idEntrega);

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

        public List<Entrega> MostrarEntregas()
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

                List<Entrega> lista = new List<Entrega>();

                while (reader.Read())
                {
                    lista.Add(
                        new Entrega{
                            Id = (int)reader["ID"],
                            IdOrdem = (int)reader["ID_ORDEM"],
                            IdFuncionario = (int)reader["ID_FUNCIONARIO"],
                            IdMotorista = (int)reader["ID_MOTORISTA"],
                           // Funcionario.Nome = (string)reader["NOME_FUNCIONARIO"],
                           // Motorista.Nome = (string)reader["NOME_MOTORISTA"],
                          //  Ordem.Destino = (string)reader["DESTINO"],
                          //  Ordem.Volume = (int)reader["VOLUME"],
                        //    Ordem.Observacao = (string)reader["OBSERVACAO"],
                        //    Ordem.Peso = (decimal)reader["PESO"],
                        //    Ordem.Observacao = (string)reader["OBSERVACAO"],
                         //   Ordem.NomeCliente = (string)reader["NOME"],
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

        public Entrega MostrarEntrega(int id)
        {
            try
            {
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
                                    WHERE E.ID = @ID_ENTREGA AND E.ATIVO > 0";

                cmd.Parameters.AddWithValue("@ID_ENTREGA", id);
                cmd.ExecuteNonQuery();

                SqlDataReader reader = cmd.ExecuteReader();

                Entrega entrega = new Entrega();

                while (reader.Read())
                {
                        
                        entrega.Id = (int)reader["ID"];
                        entrega.IdOrdem = (int)reader["ID_ORDEM"];
                        entrega.IdFuncionario = (int)reader["ID_FUNCIONARIO"];
                        entrega.IdMotorista = (int)reader["ID_MOTORISTA"];
                        entrega.Funcionario.Nome = (string)reader["NOME_FUNCIONARIO"];
                        entrega.Motorista.Nome = (string)reader["NOME_MOTORISTA"];
                        entrega.Ordem.Destino = (string)reader["DESTINO"];
                        entrega.Ordem.Volume = (int)reader["VOLUME"];
                        entrega.Ordem.Observacao = (string)reader["OBSERVACAO"];
                        entrega.Ordem.Peso = (decimal)reader["PESO"];
                        entrega.Ordem.Observacao = (string)reader["OBSERVACAO"];
                        entrega.Ordem.Cliente.Nome = (string)reader["NOME"];
                    
                }
                return entrega;
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