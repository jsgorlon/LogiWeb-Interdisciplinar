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
                                         SELECT SCOPE_IDENTITY();";
                cmd.Parameters.Clear(); 
                cmd.Parameters.AddWithValue("@id_cliente",     ordem.IdCliente);
                cmd.Parameters.AddWithValue("@id_funcionario", ordem.IdFuncionario);
                cmd.Parameters.AddWithValue("@qtd_itens",      ordem.Qtd_itens);
                cmd.Parameters.AddWithValue("@volume",         ordem.Volume);
                cmd.Parameters.AddWithValue("@peso",           ordem.Peso);
                cmd.Parameters.AddWithValue("@observacao",     ordem.Observacao ?? DBNull.Value.ToString());
           
                int id_ordem = (int)cmd.ExecuteScalar();
               // Console.WriteLine(id_ordem);

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

        public void Excluir(int id)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE ordens SET ativo = 0 WHERE id = @id";
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

        public AjaxResponse MostrarOrdens()
        {
            try{
                

                cmd.Connection = connection;
                cmd.CommandText = @"SELECT ordens.*, 
                                           funcionario.id   AS id_funcionario,
                                           funcionario.nome AS nome_funcionario,
                                           cliente.id       AS id_cliente, 
                                           cliente.nome     AS nome_cliente 
                                      FROM ordens,
                                           enderecos, 
                                           pessoas AS funcionario,
                                           pessoas AS cliente
                                     WHERE funcionario.id     = ordens.id_funcionario 
                                       AND cliente.id         = ordens.id_cliente
                                       AND enderecos.id_ordem = ordens.id";
            
                    
                SqlDataReader reader = cmd.ExecuteReader();

                List<Ordem> ordens = new List<Ordem>();
                Ordem ordem = new Ordem();
                while (reader.Read())
                {
                    
                    ordem.Id                   = (int)reader["id"];
                    ordem.IdCliente            = (int)reader["id_cliente"];
                    ordem.IdFuncionario        = (int)reader["id_funcionario"];
                    ordem.IdEndereco           = (int)reader["id_endereco"];
                    ordem.Peso                 = (decimal)reader["peso"];
                    ordem.Observacao           = (string)reader["observacao"];
                    ordem.Qtd_itens            = (short)reader["qtd_itens"];
                    ordem.Cliente.Nome         = (string)reader["nome_cliente"];
                    ordem.Funcionario.Nome     = (string)reader["nome_funcionario"];
                    ordem.Endereco.Logradouro  = (string)reader["logradouro"];
                    ordem.Endereco.Nr_casa     = (string)reader["nr_casa"];
                    ordem.Endereco.Complemento = (string)reader["complemento"];
                    ordem.Endereco.Bairro      = (string)reader["bairro"];
                    ordem.Endereco.Cep         = (string)reader["cep"];
                    ordem.Endereco.Cidade      = (string)reader["nome_cidade"];
                    ordem.Endereco.Uf          = (string)reader["sigla_uf"];
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

        public Ordem MostrarOrdem(int id)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT O.ID, O.ID_CLIENTE, O.ID_FUNCIONARIO,
                                    O.VOLUME, O.PESO, O.OBSERVACAO, O.QTD_ITENS, P.NOME NOME_CLIENTE, F.NOME NOME_FUNCIONARIO,
		                            O.ID_ENDERECO, EN.logradouro, EN.nr_casa, EN.complemento, EN.bairro, EN.cep, 
		                            EN.ID_CIDADE, CID.nome NOME_CIDADE, 
                                    CID.ID_ESTADO, EST.sigla_uf
                                    FROM ORDENS O
                                    INNER JOIN PESSOAS P
                                        ON O.ID_CLIENTE = P.ID 
                                    INNER JOIN PESSOAS F
                                        ON O.ID_FUNCIONARIO = F.ID 
									INNER JOIN cidades CID
										ON EN.id_cidade = CID.id
									INNER JOIN estados EST
										ON CID.id_estado = EST.id
                                    AND  O.ATIVO > 0 and o.id = @ID";
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();

                SqlDataReader reader = cmd.ExecuteReader();

                Ordem ordem = new Ordem();

                while (reader.Read())
                {
                        
                    ordem.Id = (int)reader["ID"];
                    ordem.Cliente.Id = (int)reader["ID_CLIENTE"];
                    ordem.Cliente.Id = (int)reader["ID_FUNCIONARIO"];
                    ordem.Endereco.Id = (int)reader["id_endereco"];
                    ordem.Volume = (string)reader["VOLUME"];
                    ordem.Peso = (decimal)reader["PESO"];
                    ordem.Observacao = (string)reader["OBSERVACAO"];
                    ordem.Qtd_itens = (short)reader["Qtd_itens"];
                    ordem.Cliente.Nome = (string)reader["NOME_CLIENTE"];
                    ordem.Funcionario.Nome = (string)reader["NOME_FUNCIONARIO"];
                    ordem.Endereco.Logradouro = (string)reader["logradouro"];
                    ordem.Endereco.Nr_casa = (string)reader["nr_casa"];
                    ordem.Endereco.Complemento = (string)reader["complemento"];
                    ordem.Endereco.Bairro = (string)reader["bairro"];
                    ordem.Endereco.Cep = (string)reader["cep"];
                    ordem.Endereco.Cidade = (string)reader["nome_cidade"];
                    ordem.Endereco.Uf = (string)reader["sigla_uf"];
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