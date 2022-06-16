using System.Data.SqlClient;
using logiWeb.Models;

namespace logiWeb.Repositories
{
    public class EnderecoSqlRepository : DBContext, IEnderecoRepository
    {
        private SqlCommand cmd = new SqlCommand();

        public void Cadastrar(Endereco endereco)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"INSERT INTO enderecos (id_cidade, cep, logradouro, nr_casa, bairro, complemento) 
                                    values (@id_cidade,@cep, @logradouro, @nr_casa, @bairro, @complemento);";

                cmd.Parameters.AddWithValue("@id_cidade", endereco.IdCidade);
                cmd.Parameters.AddWithValue("@cep", endereco.Cep);
                cmd.Parameters.AddWithValue("@logradouro", endereco.Logradouro);
                cmd.Parameters.AddWithValue("@nr_casa", endereco.Nr_casa);
                cmd.Parameters.AddWithValue("@bairro", endereco.Bairro);
                cmd.Parameters.AddWithValue("@complemento", endereco.Complemento);
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
        public Endereco MostrarEndereco(string cep, string nrCasa)
        {
            try{
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT EN.ID, logradouro, nr_casa, complemento, bairro, cep, 
		                            ID_CIDADE, CID.nome NOME_CIDADE, 
                                    CID.ID_ESTADO, EST.sigla_uf
                                    FROM ENDERECOS EN
                                 	INNER JOIN cidades CID
										ON EN.id_cidade = CID.id
									INNER JOIN estados EST
										ON CID.id_estado = EST.id 
                                    WHERE cep = @cep and nr_casa = @nrCasa";
                cmd.Parameters.AddWithValue("@cep", cep);
                cmd.Parameters.AddWithValue("@nrCasa", nrCasa);
                SqlDataReader reader = cmd.ExecuteReader();

                Endereco item = new Endereco();
                while (reader.Read())
                {
                    item.Logradouro = (string)reader["logradouro"];
                    item.Nr_casa = (string)reader["nr_casa"];
                    item.Complemento = (string)reader["complemento"];
                    item.Bairro = (string)reader["bairro"];
                    item.Cep = (string)reader["cep"];
                    item.Cidade = (string)reader["nome_cidade"];
                    item.Uf = (string)reader["sigla_uf"];
                }
                return item;
            }catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
            
        }

        public List<Endereco> MostrarEstado()
        {
            try{
                cmd.Connection = connection;
                cmd.CommandText = @"select id, nome, sigla_uf from estados";
                SqlDataReader reader = cmd.ExecuteReader();

                List<Endereco> lista = new List<Endereco>();
                
                while (reader.Read())
                {
                    Endereco item = new Endereco();
                    item.IdEstado =(int)reader["ID"];
                    item.Estado = (string)reader["nome"];
                    item.Uf = (string)reader["sigla_uf"];
                    lista.Add(item);
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
        public List<Endereco> MostrarCidade(int id_estado)
        {
            try{
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT id, nome, id_estado FROM cidades WHERE cidades.id_estado = @id_estado ORDER BY nome";
                cmd.Parameters.Clear(); 
                cmd.Parameters.AddWithValue("@id_estado", id_estado);
                SqlDataReader reader = cmd.ExecuteReader();

                List<Endereco> lista = new List<Endereco>();
                
                while (reader.Read())
                {
                    Endereco item = new Endereco();
                    item.IdCidade =(int)reader["id"];
                    item.IdEstado =(int)reader["id_estado"];
                    item.Cidade = (string)reader["nome"];
                    lista.Add(item);
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
    }
}