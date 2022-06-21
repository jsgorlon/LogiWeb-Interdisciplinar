using System.Data.SqlClient;
using logiWeb.Models;
using logiWeb.Helpers; 

namespace logiWeb.Repositories
{
    public class EntregaSqlRepository : DBContext, IEntregaRepository
    {
        private SqlCommand cmd = new SqlCommand();
        private IOrdemRepository ordemRepository;
        private IStatusRepository statusRepository;
        private AjaxResponse response = new AjaxResponse(); 

        public EntregaSqlRepository(IOrdemRepository ordemRepository,IStatusRepository statusRepository)
        {
            this.ordemRepository = ordemRepository;
            this.statusRepository = statusRepository;
        }

        public AjaxResponse Cadastrar(int id_funcionario, int id_motorista)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"INSERT INTO ENTREGAS (id_funcionario, id_motorista) 
                                    VALUES (@id_funcionario, @id_motorista);
                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                cmd.Parameters.AddWithValue("@id_funcionario",id_funcionario);
                cmd.Parameters.AddWithValue("@id_motorista",  id_motorista);
                int id_entrega = (int)cmd.ExecuteScalar();
                
                response.Message.Add("Entrega gerada com sucesso!");
                response.Item.Add("id_entrega", id_entrega);
                return response; 
                
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

        public AjaxResponse AdicionarOrdem(int id_entrega, int id_ordem){
            try{
                cmd.Connection = connection;
                cmd.CommandText = @"INSERT INTO entregas_ordens(entrega_id, ordem_id, status_id) 
                                         VALUES (@entrega_id, @ordem_id, @status_id);";
                
                cmd.Parameters.Clear(); 
                cmd.Parameters.AddWithValue("@entrega_id", id_entrega);
                cmd.Parameters.AddWithValue("@ordem_id",   id_ordem);
                cmd.Parameters.AddWithValue("@status_id",  1);
                response.Message.Add("Ordem adicionada com sucesso!");
                Console.WriteLine(id_ordem);
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

        public AjaxResponse MostrarEntregaOrdem(int id_entrega){
                try{
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT CONCAT('#', ordens.id) AS num_ordem, 
                                           pessoas.nome
                                      FROM entregas_ordens,
                                           ordens,
                                           pessoas
                                     WHERE entregas_ordens.entrega_id = @id_entrega 
                                       AND ordens.id                  = entregas_ordens.ordem_id 
                                       AND pessoas.id                 = ordens.id_cliente";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id_entrega", id_entrega);

                SqlDataReader reader = cmd.ExecuteReader();

                response.Item.Add("itens", reader.Read()); 

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

        public AjaxResponse MostrarEntregas(int? id_funcionario, int? id_motorista)
        {
            try{
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT E.ID, E.ID_FUNCIONARIO, E.ID_MOTORISTA, 
                                    F.NOME NOME_FUNCIONARIO, M.NOME NOME_MOTORISTA,
                                    SE.ID_STATUS, SE.DATA_CAD, S.NOME STATUS, S.DESCRICAO
                                    FROM ENTREGAS E
                                    INNER JOIN PESSOAS F
                                        ON E.ID_FUNCIONARIO = F.ID 
                                    INNER JOIN PESSOAS M
                                        ON E.ID_MOTORISTA = M.ID 
                                    INNER JOIN status_entrega SE
                                        ON E.ID = SE.ID_ENTREGA
                                    INNER JOIN STATUS S
                                        ON SE.ID_STATUS = S.ID
                                    WHERE E.ATIVO > 0
                ";
                if(id_funcionario != null)
                {
                    cmd.CommandText += " AND e.id_funcionario = @id_funcionario";
                    cmd.Parameters.AddWithValue("@id_funcionario", id_funcionario);
                }
                    
            
                if(id_motorista != null)
                {
                    cmd.CommandText += " and e.id_motorista = @id_motorista";
                    cmd.Parameters.AddWithValue("@id_motorista", id_motorista);
                }

                SqlDataReader reader = cmd.ExecuteReader();

                List<Entrega> entregas = new List<Entrega>();

                while (reader.Read())
                {
                    Entrega entrega = new Entrega();
                            entrega.Id = (int)reader["ID"];
                            entrega.IdFuncionario = (int)reader["ID_FUNCIONARIO"];
                            entrega.IdMotorista = (int)reader["ID_MOTORISTA"];
                            entrega.Funcionario.Nome = (string)reader["NOME_FUNCIONARIO"];
                            entrega.Motorista.Nome = (string)reader["NOME_MOTORISTA"];
                            entrega.Status.Id = (short)reader["ID_STATUS"];
                            entrega.DataCadastro = (DateTime)reader["DATA_CAD"];
                            entrega.Status.Nome = (string)reader["STATUS"];
                            entrega.Status.Descricao = (string)reader["DESCRICAO"];
                    entregas.Add(entrega);
                }
                response.Item.Add("entregas", entregas); 
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

        public Entrega MostrarEntrega(int id)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT E.ID, E.ID_ORDEM, E.ID_FUNCIONARIO, E.ID_MOTORISTA, 
                                    F.NOME NOME_FUNCIONARIO, M.NOME NOME_MOTORISTA,
                                    SE.ID_STATUS, SE.DATA_CAD, S.NOME STATUS, S.DESCRICAO
                                    FROM ENTREGAS E
                                    INNER JOIN PESSOAS F
                                        ON E.ID_FUNCIONARIO = F.ID 
                                    INNER JOIN PESSOAS M
                                        ON E.ID_MOTORISTA = M.ID 
                                    INNER JOIN status_entrega SE
                                        ON E.ID = SE.ID_ENTREGA
                                    INNER JOIN STATUS S
                                        ON SE.ID_STATUS = S.ID                          
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
                    entrega.Status.Id = (short)reader["ID_STATUS"];
                    entrega.DataCadastro = (DateTime)reader["DATA_CAD"];
                    entrega.Status.Nome = (string)reader["STATUS"];
                    entrega.Status.Descricao = (string)reader["DESCRICAO"];               
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
        public Entrega MostrarDetalheEntrega(int id)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT ent.id, ent.entrega_id, ent.ordem_id, ent.status_id,
                                    e.id_funcionario, e.id_motorista,
                                    p.nome nome_motorista,
                                    f.nome nome_funcionario,
                                    o.peso, o.volume, o.qtd_itens, o.observacao,
                                    c.nome nome_cliente, c.telefone,
                                    EN.logradouro, EN.nr_casa, EN.complemento, EN.bairro, EN.cep, 
                                    EN.ID_CIDADE, CID.nome NOME_CIDADE, 
                                    CID.ID_ESTADO, EST.NOME NOME_ESTADO EST.sigla_uf,
                                    s.nome, s.descricao
                                    FROM entregas_ordens ent
                                    inner join entregas e
                                        on e.id = ent.entrega_id
                                    inner join pessoas p
                                        on p.id = e.id_motorista
                                    inner join pessoas f
                                        on f.id = e.id_funcionario
                                    inner join ordens o
                                        on o.id = ent.ordem_id
                                    inner join pessoas c
                                        on c.id = o.id_cliente
                                    INNER JOIN enderecos EN
                                        ON O.id_endereco = EN.id
                                    INNER JOIN cidades CID
                                        ON EN.id_cidade = CID.id
                                    INNER JOIN estados EST
                                        ON CID.id_estado = EST.id
                                    inner join status s
                                        on s.id = ent.status
                                    where ent.entrega_id = @ID_ENTREGA ";

                cmd.Parameters.AddWithValue("@ID_ENTREGA", id);
                cmd.ExecuteNonQuery();

                SqlDataReader reader = cmd.ExecuteReader();

                Entrega entrega = new Entrega();

                while (reader.Read())
                {
                        
                    entrega.Id = (int)reader["entrega_id"];
                    entrega.IdOrdem = (int)reader["entrega_id"];
                    entrega.IdFuncionario = (int)reader["ID_FUNCIONARIO"];
                    entrega.IdMotorista = (int)reader["ID_MOTORISTA"];
                    entrega.Funcionario.Nome = (string)reader["NOME_FUNCIONARIO"];
                    entrega.Motorista.Nome = (string)reader["NOME_MOTORISTA"];
                    entrega.Ordem.Volume = (string)reader["VOLUME"];
                    entrega.Ordem.Observacao = (string)reader["OBSERVACAO"];
                    entrega.Ordem.Peso = (decimal)reader["PESO"];
                    entrega.Ordem.Qtd_itens =  (short)reader["qtd_itens"];
                    entrega.Ordem.Cliente.Nome = (string)reader["NOME_CLIENTE"];
                    entrega.Ordem.Cliente.Telefone = (string)reader["TELEFONE"];
                    entrega.Ordem.Endereco.Id = (int)reader["ID_ENDERECO"];
                    entrega.Ordem.Endereco.Logradouro =  (string)reader["LOGRADOURO"];
                    entrega.Ordem.Endereco.Nr_casa =  (string)reader["NR_CASA"];
                    entrega.Ordem.Endereco.Bairro =  (string)reader["BAIRRO"];
                    entrega.Ordem.Endereco.Complemento =  (string)reader["COMPLEMENTO"];
                    entrega.Ordem.Endereco.Cep =  (string)reader["CEP"];
                    entrega.Ordem.Endereco.IdCidade = (int)reader["ID_CIDADE"];
                    entrega.Ordem.Endereco.Cidade =  (string)reader["NOME_CIDADE"];
                    entrega.Ordem.Endereco.IdEstado = (int)reader["ID_ESTADO"];
                    entrega.Ordem.Endereco.Estado =  (string)reader["NOME_ESTADO"];
                    entrega.Ordem.Endereco.Uf =  (string)reader["SIGLA_UF"];
                    
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
        public void StatusOrdem(Ordem ordem)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE entregas_ordens SET STATUS = @ID_STATUS 
                                    WHERE ordem_id = @ID_ORDEM";
                cmd.Parameters.AddWithValue("@ID_STATUS", ordem.IdStatus);
                cmd.Parameters.AddWithValue("@ID_ORDEM", ordem.Id);
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
        public void StatusEntrega(Entrega entrega)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE status_entrega SET ID_STATUS = @ID_STATUS 
                                    WHERE ID_ENTREGA = @ID_ENTREGA";
                cmd.Parameters.AddWithValue("@ID_STATUS", entrega.IdStatus);
                cmd.Parameters.AddWithValue("@ID_ENTREGA", entrega.Id);
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