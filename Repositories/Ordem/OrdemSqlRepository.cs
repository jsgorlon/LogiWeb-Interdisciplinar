using System.Data.SqlClient;
using logiWeb.Models;
using logiWeb.Helpers; 

namespace logiWeb.Repositories
{
    public class OrdemSqlRepository : DBContext, IOrdemRepository
    {
        private SqlCommand cmd = new SqlCommand();
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

        public  AjaxResponse GetById(int id){
            try{
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT ordens.*, 
                                           ordens.id AS id_pedido, 
                                           enderecos.*,
                                           funcionario.id   AS id_funcionario,
                                           funcionario.nome AS nome_funcionario,
                                           cliente.id       AS id_cliente, 
                                           cliente.nome     AS nome_cliente,
                                           cidades.id_estado,
                                           cidades.nome AS nome_cidade, 
                                           estados.nome as nome_estado
                                      FROM ordens,
                                           enderecos, 
                                           pessoas AS funcionario,
                                           pessoas AS cliente,
                                           cidades,
                                           estados 
                                     WHERE ordens.id = @id
                                       AND ordens.ativo = 1
                                       AND funcionario.id     = ordens.id_funcionario 
                                       AND cliente.id         = ordens.id_cliente
                                       AND enderecos.id_ordem = ordens.id 
                                       AND cidades.id = enderecos.id_cidade
                                       AND estados.id = cidades.id_estado";
            
                cmd.Parameters.Clear(); 
                cmd.Parameters.AddWithValue("@id", id);

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
                    ordem.Endereco.Cidade      = (string)reader["nome_cidade"];
                    ordem.Endereco.Estado      = (string)reader["nome_estado"];
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
    }
}