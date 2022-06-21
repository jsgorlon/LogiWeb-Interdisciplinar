using System.Data.SqlClient;
using logiWeb.Models;
using logiWeb.Helpers; 

namespace logiWeb.Repositories
{
    public class OrdemSqlRepository : DBContext, IOrdemRepository
    {
        
        private AjaxResponse response = new AjaxResponse(); 

        private IClienteRepository clienteRepository;
        public OrdemSqlRepository(IClienteRepository clienteRepository)
        {
           // this.clienteRepository = clienteRepository;
        }

        public AjaxResponse Cadastrar(Ordem ordem, Endereco endereco)
        {
          
            try
            {   
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"INSERT INTO ordens (id_cliente, id_funcionario, qtd_itens, volume, peso, observacao)  
                                         VALUES (@id_cliente, @id_funcionario, @qtd_itens, @volume, @peso, @observacao);
                                         SELECT CAST(SCOPE_IDENTITY() AS INT);";
                cmd.Parameters.Clear(); 
                cmd.Parameters.AddWithValue("@id_cliente",     ordem.IdCliente);
                cmd.Parameters.AddWithValue("@id_funcionario", ordem.IdFuncionario);
                cmd.Parameters.AddWithValue("@qtd_itens",      ordem.Qtd_itens);
                cmd.Parameters.AddWithValue("@volume",         ordem.Volume);
                cmd.Parameters.AddWithValue("@peso",           ordem.Peso);
                cmd.Parameters.AddWithValue("@observacao",     ordem.Observacao ?? DBNull.Value.ToString());
           
                int id_ordem = (int)cmd.ExecuteScalar();
             
                cmd.CommandText = @"INSERT INTO enderecos(id_ordem, id_cidade, cep, logradouro, nr_casa, bairro, complemento)
                                          VALUES (@id_ordem, @id_cidade, @cep, @logradouro, @nr_casa, @bairro, @complemento)";
                cmd.Parameters.Clear(); 
                cmd.Parameters.AddWithValue("@id_ordem",     id_ordem);
                cmd.Parameters.AddWithValue("@id_cidade",    endereco.IdCidade);
                cmd.Parameters.AddWithValue("@cep",          endereco.Cep);
                cmd.Parameters.AddWithValue("@nr_casa",      endereco.Nr_casa);
                cmd.Parameters.AddWithValue("@bairro",       endereco.Bairro);
                cmd.Parameters.AddWithValue("@logradouro",   endereco.Logradouro  ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@complemento",  endereco.Complemento ?? DBNull.Value.ToString());
                cmd.ExecuteNonQuery();

                response.Message.Add("Ordem criada com sucesso!");
                

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

        public AjaxResponse AlterarStatus(int id, int status)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE ordens SET ativo = @status WHERE id = @id";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.ExecuteNonQuery();
                
                response.Message.Add("Status da ordem alterado com sucesso!");
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

        public AjaxResponse MostrarOrdens(int? id_funcionario,string? nome_cliente, int? status)
        {
            try{
                 SqlCommand cmd = new SqlCommand();

                string query = "";
               
                if(nome_cliente != null)
                    query += " AND cliente.nome LIKE @nome";

                if(status != null)
                  query += " AND ordens.ativo = @status"; 
                
                if(id_funcionario != null)
                  query += " AND ordens.id_funcionario = @id_funcionario"; 

              

                cmd.Connection = connection;
                cmd.CommandText = @"SELECT ordens.*, 
                                           ordens.id AS id_pedido, 
                                           enderecos.*,
                                           funcionario.id   AS id_funcionario,
                                           funcionario.nome AS nome_funcionario,
                                           cliente.id       AS id_cliente, 
                                           cliente.nome     AS nome_cliente,
                                           cidades.id_estado 
                                      FROM ordens,
                                           enderecos, 
                                           pessoas AS funcionario,
                                           pessoas AS cliente,
                                           cidades 
                                     WHERE funcionario.id     = ordens.id_funcionario 
                                       AND cliente.id         = ordens.id_cliente
                                       AND enderecos.id_ordem = ordens.id 
                                       AND cidades.id = enderecos.id_cidade  "+query+" ORDER BY ordens.id DESC";
            
                cmd.Parameters.Clear(); 
                
                if(nome_cliente != null)
                    cmd.Parameters.AddWithValue("@nome", "%"+nome_cliente.ToUpper()+"%");
            
                if(status != null)
                    cmd.Parameters.AddWithValue("@status", status); 
                
                if(id_funcionario != null)
                   cmd.Parameters.AddWithValue("@id_funcionario", id_funcionario); 

                SqlDataReader reader = cmd.ExecuteReader();

                List<Ordem> ordens = new List<Ordem>();
                

                while (reader.Read())
                {
                    Ordem ordem = new Ordem();
                    ordem.Id                   = (int)reader["id_ordem"];
                    ordem.IdCliente            = (int)reader["id_cliente"];
                    ordem.IdFuncionario        = (int)reader["id_funcionario"];
                    ordem.Peso                 = (decimal)reader["peso"];
                    ordem.Observacao           = (string)reader["observacao"];
                    ordem.Qtd_itens            = (short)reader["qtd_itens"];
                    ordem.Ativo                = (bool)reader["ativo"];
                    ordem.Cliente.Nome         = (string)reader["nome_cliente"];
                    ordem.Funcionario.Nome     = (string)reader["nome_funcionario"];
                    ordem.Endereco.IdEstado    = (int)reader["id_estado"]; 
                    ordem.Endereco.IdCidade    = (int)reader["id_cidade"];
                    ordem.Endereco.Logradouro  = (string)reader["logradouro"];
                    ordem.Endereco.Nr_casa     = (string)reader["nr_casa"];
                    ordem.Endereco.Complemento = (string)reader["complemento"];
                    ordem.Endereco.Bairro      = (string)reader["bairro"];
                    ordem.Endereco.Cep         = (string)reader["cep"];
                    ordens.Add(ordem);
                }

                response.Item.Add("ordens", ordens); 
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

        public AjaxResponse MostrarOrdem(int id_ordem)
        {
            try{
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT O.ID, O.ID_CLIENTE, O.ID_FUNCIONARIO, coalesce(eo.status_id,0) status_id,  
                                    O.VOLUME, O.PESO, O.OBSERVACAO, O.QTD_ITENS, P.NOME NOME_CLIENTE, F.NOME NOME_FUNCIONARIO,
		                            O.id, EN.logradouro, EN.nr_casa, EN.complemento, EN.bairro, EN.cep, 
		                            EN.ID_CIDADE, CID.nome NOME_CIDADE, 
                                    CID.ID_ESTADO, EST.sigla_uf
                                    FROM ORDENS O
                                    INNER JOIN PESSOAS P
                                        ON O.ID_CLIENTE = P.ID 
                                    INNER JOIN PESSOAS F
                                        ON O.ID_FUNCIONARIO = F.ID 
									INNER JOIN enderecos EN
										ON O.id = EN.id_ordem
									INNER JOIN cidades CID
										ON EN.id_cidade = CID.id
									INNER JOIN estados EST
										ON CID.id_estado = EST.id
                                    left join entregas_ordens eo
									  on eo.ordem_id = o.id
                                    where  O.ATIVO > 0 and o.id = @ID";
                cmd.Parameters.AddWithValue("@ID", id_ordem);              

                SqlDataReader reader = cmd.ExecuteReader();

                Ordem ordem = new Ordem();

                while (reader.Read())
                {
                        
                    ordem.Id = (int)reader["ID"];
                    ordem.Endereco.Logradouro = (string)reader["logradouro"];
                    ordem.Endereco.Nr_casa = (string)reader["nr_casa"];
                    ordem.Endereco.Complemento = (string)reader["complemento"];
                    ordem.Endereco.Bairro = (string)reader["bairro"];
                    ordem.Endereco.Cep = (string)reader["cep"];
                    ordem.Endereco.Cidade = (string)reader["nome_cidade"];
                    ordem.Endereco.Uf = (string)reader["sigla_uf"];
                    ordem.IdStatus = (short)Convert.ToInt16(reader["status_id"]);
                }
                if (ordem.Id == 0)
                { 
                    response.Error = true; 
                    response.Message.Add("Ordem não encontrada!");
                }
                response.Item.Add("ordens", ordem); 
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