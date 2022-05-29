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
                cmd.CommandText = @"INSERT INTO ORDENS (id_cliente, id_endereco, id_funcionario, qtd_itens, volume, peso, observacao)  
                                    VALUES (@id_cliente, @id_endereco, @id_funcionario, @qtd_itens, @volume, @peso, @observacao);";

                cmd.Parameters.AddWithValue("@id_cliente", ordem.Cliente.Id);
                cmd.Parameters.AddWithValue("@id_cliente", ordem.Endereco.Id);
                cmd.Parameters.AddWithValue("@id_cliente", ordem.Funcionario.Id);
                cmd.Parameters.AddWithValue("@id_cliente", ordem.Qtd_itens);
                cmd.Parameters.AddWithValue("@destino", ordem.Endereco.Id);
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
                cmd.CommandText = @"SELECT O.ID, O.ID_CLIENTE, O.ID_FUNCIONARIO, O.id_endereco, 
                                    O.VOLUME, O.PESO, O.OBSERVACAO, O.QTD_ITENS, P.NOME NOME_CLIENTE, F.NOME NOME_FUNCIONARIO,
		                            O.ID_ENDERECO, EN.logradouro, EN.nr_casa, EN.complemento, EN.bairro, EN.cep, 
		                            EN.ID_CIDADE, CID.nome NOME_CIDADE, 
                                    CID.ID_ESTADO, EST.sigla_uf
                                    FROM ORDENS O
                                    INNER JOIN PESSOAS P
                                        ON O.ID_CLIENTE = P.ID 
                                    INNER JOIN PESSOAS F
                                        ON O.ID_FUNCIONARIO = F.ID 
									INNER JOIN enderecos EN
										ON O.id_endereco = EN.id
									INNER JOIN cidades CID
										ON EN.id_cidade = CID.id
									INNER JOIN estados EST
										ON CID.id_estado = EST.id
                                    WHERE O.ATIVO > 0 ";

                SqlDataReader reader = cmd.ExecuteReader();

                List<Ordem> lista = new List<Ordem>();
                Ordem item = new Ordem();
                while (reader.Read())
                {
                    
                    item.Id = (int)reader["ID"];
                    item.Cliente.Id = (int)reader["ID_CLIENTE"];
                    item.Cliente.Id = (int)reader["ID_FUNCIONARIO"];
                    item.Endereco.Id =(int)reader["id_endereco"];
                    item.Peso = (decimal)reader["PESO"];
                    item.Observacao = (string)reader["OBSERVACAO"];
                    item.Qtd_itens = (short)reader["Qtd_itens"];
                    item.Cliente.Nome = (string)reader["NOME_CLIENTE"];
                    item.Funcionario.Nome = (string)reader["NOME_FUNCIONARIO"];
                    item.Endereco.Logradouro = (string)reader["logradouro"];
                    item.Endereco.Nr_casa = (string)reader["nr_casa"];
                    item.Endereco.Complemento = (string)reader["complemento"];
                    item.Endereco.Bairro = (string)reader["bairro"];
                    item.Endereco.Cep = (string)reader["cep"];
                    item.Endereco.Cidade = (string)reader["nome_cidade"];
                    item.Endereco.Uf = (string)reader["sigla_uf"];
                    lista.Add(item);
                }
                return lista;
            }catch(Exception ex)
            {
                Console.WriteLine("aqui " + ex.Message);
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
                cmd.CommandText = @"SELECT O.ID, O.ID_CLIENTE, O.ID_FUNCIONARIO, O.id_endereco, 
                                    O.VOLUME, O.PESO, O.OBSERVACAO, O.QTD_ITENS, P.NOME NOME_CLIENTE, F.NOME NOME_FUNCIONARIO,
		                            O.ID_ENDERECO, EN.logradouro, EN.nr_casa, EN.complemento, EN.bairro, EN.cep, 
		                            EN.ID_CIDADE, CID.nome NOME_CIDADE, 
                                    CID.ID_ESTADO, EST.sigla_uf
                                    FROM ORDENS O
                                    INNER JOIN PESSOAS P
                                        ON O.ID_CLIENTE = P.ID 
                                    INNER JOIN PESSOAS F
                                        ON O.ID_FUNCIONARIO = F.ID 
									INNER JOIN enderecos EN
										ON O.id_endereco = EN.id
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