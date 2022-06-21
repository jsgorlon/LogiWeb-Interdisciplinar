using System.Data.SqlClient;
using logiWeb.Models;
using logiWeb.Helpers; 

namespace logiWeb.Repositories
{
    public class EntregaSqlRepository : DBContext, IEntregaRepository
    {
       
        private IOrdemRepository ordemRepository;
        private IStatusRepository statusRepository;
        private AjaxResponse response = new AjaxResponse(); 

        public EntregaSqlRepository(IOrdemRepository ordemRepository,IStatusRepository statusRepository)
        {
            this.ordemRepository = ordemRepository;
            this.statusRepository = statusRepository;
        }

        public AjaxResponse Cadastrar(int IdFuncionario, int idMotorista, int[] idOrdem)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"INSERT INTO ENTREGAS (id_funcionario, id_motorista) 
                                    VALUES (@id_funcionario, @id_motorista);
                                    SELECT SCOPE_IDENTITY();  ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id_funcionario", IdFuncionario);
                cmd.Parameters.AddWithValue("@id_motorista", idMotorista);
                int idEntrega = Convert.ToInt32((decimal)cmd.ExecuteScalar());
                
                //status padrao para entrega 12 - pendente
                cmd.CommandText = @"INSERT INTO status_entrega (id_entrega, id_status) 
                                    VALUES (@id_entrega, 12); ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id_entrega", idEntrega);
                cmd.ExecuteNonQuery();
                 //status padrao para ordem 1 - pendente
                foreach (var idOrd in idOrdem)
                {
                    cmd.CommandText = @"INSERT INTO entregas_ordens (ordem_id, entrega_id, status_id) 
                                        VALUES (@id_ordem,  @id_entrega , 1); ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id_ordem", idOrd);
                    cmd.Parameters.AddWithValue("@id_entrega", idEntrega);

                    cmd.ExecuteNonQuery();
                }
                response.Message.Add("Entrega adicionada com sucesso!");
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

        public AjaxResponse Excluir(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE ENTREGAS SET ATIVO = 0 WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
                response.Message.Add("Entrega excluída com sucesso!");
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

        public AjaxResponse MostrarEntregas(int? id_funcionario, int? id_motorista)
        {
            try{
                SqlCommand cmd = new SqlCommand();
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
                    cmd.CommandText += " and e.id_funcionario = @id_funcionario";
                    cmd.Parameters.AddWithValue("@id_funcionario", id_funcionario);
                }
                    
            
                if(id_motorista != null)
                {
                    cmd.CommandText += " and e.id_motorista = @id_motorista";
                    cmd.Parameters.AddWithValue("@id_motorista", id_motorista);
                }

                SqlDataReader reader = cmd.ExecuteReader();

                List<Entrega> lista = new List<Entrega>();

                while (reader.Read())
                {
                    Entrega item = new Entrega();
                    item.Id = (int)reader["ID"];
                    item.IdFuncionario = (int)reader["ID_FUNCIONARIO"];
                    item.IdMotorista = (int)reader["ID_MOTORISTA"];
                    item.Funcionario.Nome = (string)reader["NOME_FUNCIONARIO"];
                    item.Motorista.Nome = (string)reader["NOME_MOTORISTA"];
                    item.Status.Id = (short)reader["ID_STATUS"];
                    item.DataCadastro = (DateTime)reader["DATA_CAD"];
                    item.Status.Nome = (string)reader["STATUS"];
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

        public List<Ordem> MostrarOrdensEntrega(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT ordem_id, status_id from entregas_ordens where entrega_id = @id_entreg";

                cmd.Parameters.AddWithValue("@ID_ENTREG", id);
                cmd.ExecuteNonQuery();

                SqlDataReader reader = cmd.ExecuteReader();
                List<Ordem> lista = new List<Ordem>();
                

                while (reader.Read())
                {       
                    Ordem ordem = new Ordem();                 
                    ordem.Id = (int)reader["ordem_id"];
                    ordem.IdStatus= (short)reader["status_id"];
                    lista.Add(ordem);           
                }
                
                return lista;
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
        public AjaxResponse MostrarDetalheEntrega(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT eo.status_id,  s.id stats_id, s.nome nome_status, o.id, o.qtd_itens, o.peso, o.volume, o.observacao,
                                    e.bairro, e.cep, e.complemento, e.logradouro, e.nr_casa,
                                    cid.id id_cidade, cid.nome nome_cidade,
                                    est.id id_estado, est.nome nome_estado,
                                    p.nome nome_cliente
                                    from entregas_ordens eo
                                        inner join status s
                                            on s.id = eo.status_id
                                        inner join ordens o
                                            on o.id = eo.ordem_id
                                        inner join enderecos e
                                            on e.id_ordem = o.id
                                        inner join cidades cid
                                            on e.id_cidade = cid.id
                                        inner join estados est
                                            on cid.id_estado = est.id
                                        inner join clientes c
                                            on o.id_cliente = c.id_pessoa
                                        inner join pessoas p
		                                    on c.id_pessoa = p.id
                                        where eo.entrega_id =@ID_ENTREGA ";

                cmd.Parameters.AddWithValue("@ID_ENTREGA", id);
                cmd.ExecuteNonQuery();

                SqlDataReader reader = cmd.ExecuteReader();

                List<Ordem> lista = new List<Ordem>();
                
                while (reader.Read())
                {
                    Ordem ordem = new Ordem();
                    ordem.Id = (int)reader["id"];
                    ordem.Qtd_itens =  (short)reader["qtd_itens"];
                    ordem.Peso = (decimal)reader["PESO"];
                    ordem.Volume = (string)reader["VOLUME"];
                    ordem.Observacao = (string)reader["OBSERVACAO"];
                    ordem.Cliente.Nome = (string)reader["NOME_CLIENTE"];
                    ordem.Endereco.Bairro =  (string)reader["BAIRRO"];
                    ordem.Endereco.Cep =  (string)reader["CEP"];
                    ordem.Endereco.Complemento =  (string)reader["COMPLEMENTO"];
                    ordem.Endereco.Logradouro =  (string)reader["LOGRADOURO"];
                    ordem.Endereco.Nr_casa =  (string)reader["NR_CASA"];
                    ordem.Endereco.IdCidade = (int)reader["ID_CIDADE"];
                    ordem.Endereco.Cidade =  (string)reader["NOME_CIDADE"];
                    ordem.Endereco.IdEstado = (int)reader["ID_ESTADO"];
                    ordem.Endereco.Estado =  (string)reader["NOME_ESTADO"];
                    ordem.Status.Nome = (string)reader["nome_status"];
                    ordem.IdStatus = (short)reader["stats_id"];
                    lista.Add(ordem);
                    
                }
                response.Item.Add("ordens", lista); 
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
        public AjaxResponse StatusOrdem(int id_ordem, int id_status, int id_entrega)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE entregas_ordens SET STATUS_id = @ID_STATUS 
                                    WHERE ordem_id = @ID_ORDEM and entrega_id = @ID_ENTREGA";
                cmd.Parameters.AddWithValue("@ID_STATUS", id_status);
                cmd.Parameters.AddWithValue("@ID_ORDEM",id_ordem);
                cmd.Parameters.AddWithValue("@ID_ENTREGA", id_entrega);
                cmd.ExecuteNonQuery();
                response.Message.Add("Status alterado com sucesso!");
                return response;
            }
              catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
               // Dispose();
            }
        }
         public void StatusEntrega(int id_entrega, int id_status)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.Connection.Open();
                cmd.Parameters.Clear();
                cmd.CommandText = @"UPDATE status_entrega SET ID_STATUS = @ID_STAT 
                                    WHERE ID_ENTREGA = @ID_ENTRE";
                cmd.Parameters.AddWithValue("@ID_STAT", id_status);
                cmd.Parameters.AddWithValue("@ID_ENTRE",id_entrega);
                cmd.ExecuteNonQuery();
                Dispose();
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