using System.Data.SqlClient;
using logiWeb.Models;
using logiWeb.Helpers; 

namespace logiWeb.Repositories
{
    public class ClienteSqlRepository : DBContext, IClienteRepository
    {
      
        private AjaxResponse response = new AjaxResponse(); 
        private SqlCommand cmd = new SqlCommand();
        public AjaxResponse Cadastrar(Cliente cliente)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT pessoas.id 
                                      FROM pessoas 
                                     WHERE cpf = @cpf;";
                cmd.Parameters.Clear(); 
                cmd.Parameters.AddWithValue("@cpf", cliente.Cpf);
                SqlDataReader reader = cmd.ExecuteReader();

                int id_pessoa = reader.Read() ? (int)reader["id"] : 0;
                
                reader.Dispose(); 
                
                if (id_pessoa == 0)
                {
                    
                    cmd.Connection = connection;
                    cmd.CommandText = @"INSERT INTO pessoas (nome, cpf, rg, data_nasc, telefone, email) 
                                            VALUES (@nome, @cpf, @rg, @data_nasc, @telefone, @email);
                                            SELECT SCOPE_IDENTITY();";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@nome",      cliente.Nome.ToUpper());
                    cmd.Parameters.AddWithValue("@cpf",       cliente.Cpf);
                    cmd.Parameters.AddWithValue("@data_nasc", cliente.DatNasc);
                    cmd.Parameters.AddWithValue("@rg",        cliente.Rg       ?? DBNull.Value.ToString());
                    cmd.Parameters.AddWithValue("@email",     cliente.Email    ?? DBNull.Value.ToString());
                    cmd.Parameters.AddWithValue("@telefone",  cliente.Telefone ?? DBNull.Value.ToString());
                
                    id_pessoa = (int)cmd.ExecuteScalar();
                }
                else
                {
                  cmd.CommandText = @"SELECT id_pessoa
                                        FROM clientes 
                                       WHERE id_pessoa = @id_pessoa";
                  cmd.Parameters.AddWithValue("@id_pessoa", id_pessoa);
                  reader = cmd.ExecuteReader();
                 
                  if(reader.HasRows)
                  {
                     response.Message.Add("Este CPF já está associado a um cliente.");
                     response.Error = true; 
                  }
                  reader.Dispose(); 
                }

                if(this.response.Error)
                    return this.response; 

                cmd.CommandText = @"INSERT INTO clientes (id_pessoa) VALUES (@id_pessoa)";
                cmd.Parameters.Clear(); 
                cmd.Parameters.AddWithValue("@id_pessoa", id_pessoa);
                cmd.ExecuteNonQuery();

                this.response.Message.Add("Cliente cadastrado com sucesso!");
                
                return this.response; 
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public AjaxResponse Mostrar(string? nome, int? status)
        {
            try
            {
                
                List<Cliente> clientes = new List<Cliente>();
                string query = "";
                List<string> query_parameters = new List<string>(); 

                if(nome != null)
                    query_parameters.Add("pessoas.nome LIKE @nome"); 

                if(status != null)
                  query_parameters.Add("clientes.ativo = @status"); 

                if(query_parameters.Count > 0)
                   query = " WHERE " + String.Join(" AND ", query_parameters); 
             

                cmd.Connection = connection;
                cmd.CommandText = @"SELECT id_pessoa, nome, cpf, rg, data_nasc, email, telefone, dat_cad, ativo
                                      FROM clientes
                                INNER JOIN pessoas 
                                        ON id_pessoa = id "+query;
                cmd.Parameters.Clear(); 

                if(nome != null)
                    cmd.Parameters.AddWithValue("@nome", "%"+nome.ToUpper()+"%"); 
                
                if(status != null)
                   cmd.Parameters.AddWithValue("@status", status); 

                SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                    clientes.Add( new Cliente{
                                                Id = (int)reader["id_pessoa"],
                                                Nome = (string)reader["nome"],
                                                Cpf = (string)reader["cpf"],
                                                Rg = (string)reader["rg"],
                                                DatNasc = (DateTime)reader["data_nasc"],
                                                Email = (string)reader["email"],
                                                Telefone = (string)reader["telefone"],
                                                DatCad = (DateTime)reader["dat_cad"],
                                                Ativo    =  (bool)reader["ativo"],
                                            });
                


                response.Item.Add("clientes",  clientes);
                reader.Close(); 
                
                return response; 
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public AjaxResponse Atualizar(int id, Cliente cliente)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE pessoas
                                    SET nome = @nome, 
                                         cpf = @cpf, 
                                          rg = @rg, 
                                   data_nasc = @data_nasc, 
                                    telefone = @telefone, 
                                       email = @email
                                    WHERE id = @id
                                ";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nome",      cliente.Nome.ToUpper());
                cmd.Parameters.AddWithValue("@cpf",       cliente.Cpf);
                cmd.Parameters.AddWithValue("@data_nasc", cliente.DatNasc);
                cmd.Parameters.AddWithValue("@rg",        cliente.Rg       ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@email",     cliente.Email    ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@telefone",  cliente.Telefone ?? DBNull.Value.ToString());
                
                cmd.ExecuteNonQuery();

                response.Message.Add("Cliente atualizado com sucesso!");
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return response; 
        }

        public AjaxResponse AlterarStatus(int id, int status)
        {
            try
            {
                
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE clientes
                                       SET clientes.ativo = @status
                                     WHERE clientes.id_pessoa = @id";
                cmd.Parameters.Clear(); 
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.ExecuteNonQuery();

                response.Message.Add(status == StatusCliente.Ativo ? "Cliente ativado com sucesso!" : "Cliente inativado com sucesso!");

                return response; 
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
