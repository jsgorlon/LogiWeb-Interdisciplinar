using System.Data.SqlClient;
using logiWeb.Models;

namespace logiWeb.Repositories
{
    public class ClienteSqlRepository : DBContext, IClienteRepository
    {
      
        private SqlCommand cmd = new SqlCommand();
        public void Cadastrar(Cliente cliente)
        {
            try
            {
           
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT pessoas.id 
                                      FROM pessoas 
                                     WHERE cpf = @cpf;";

                cmd.Parameters.AddWithValue("@cpf", cliente.Cpf);
                SqlDataReader reader = cmd.ExecuteReader();

                int id_pessoa = reader.Read() ? (int)reader["id"] : 0;
                
                reader.Dispose(); 
                
                string errorMsg = ""; 
                
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
                  cmd.CommandText = @"SELECT COUNT(1) > 0 as cliente_cadastrado 
                                        FROM clientes 
                                       WHERE id_pessoa = @id_pessoa";
                  cmd.Parameters.AddWithValue("@id_pessoa", id_pessoa);
                  bool cliente_cadastrado = (bool)reader["cliente_cadastrado"];
                  reader.Dispose();
                  
                  if(cliente_cadastrado)
                    errorMsg += "Este CPF já está associado a um cliente.";
                }

                

                cmd.CommandText = @"INSERT INTO clientes (id_pessoa) VALUES (@id_pessoa)";
                cmd.Parameters.AddWithValue("@id_pessoa", id_pessoa);
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

        public List<Cliente> Mostrar()
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT id_pessoa, nome, cpf, rg, data_nasc, email, telefone, dat_cad, ativo
                                    FROM clientes
                                    INNER JOIN pessoas ON id_pessoa = id
                                    WHERE ativo = 1";

                SqlDataReader reader = cmd.ExecuteReader();

                List<Cliente> lista = new List<Cliente>();

                while (reader.Read())
                {
                    lista.Add(
                        new Cliente
                        {
                            Id = (int)reader["id_pessoa"],
                            Nome = (string)reader["nome"],
                            Cpf = (string)reader["cpf"],
                            Rg = (string)reader["rg"],
                            DatNasc = (DateTime)reader["data_nasc"],
                            Email = (string)reader["email"],
                            Telefone = (string)reader["telefone"],
                            DatCad = (DateTime)reader["dat_cad"],
                            Ativo = true
                        }
                    );
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

        public Cliente Mostrar(int id)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT id_pessoa, nome, cpf, rg, data_nasc, email, telefone, dat_cad, ativo
                                      FROM clientes
                                INNER JOIN pessoas ON id_pessoa = id
                                     WHERE id_pessoa = @id 
                                       AND ativo = 1";

                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    return new Cliente 
                    {
                        Id = (int)reader["id_pessoa"],
                        Nome = (string)reader["nome"],
                        Cpf = (string)reader["cpf"],
                        Rg = (string)reader["rg"],
                        DatNasc = (DateTime)reader["data_nasc"],
                        Email = (string)reader["email"],
                        Telefone = (string)reader["telefone"],
                        DatCad = (DateTime)reader["dat_cad"],
                        Ativo = true
                    };
                }
                else
                {
                    return null;
                }
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

        public List<Cliente> MostrarPorCpf(string cpf)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT id_pessoa, nome, cpf, rg, data_nasc, email, telefone, dat_cad, ativo
                                    FROM clientes
                                    INNER JOIN pessoas ON id_pessoa = id
                                    WHERE cpf LIKE @nome_ou_cpf and ativo = 1";
                                    
                cmd.Parameters.AddWithValue(@"nome_ou_cpf", string.Format("%{0}%", cpf));

                SqlDataReader reader = cmd.ExecuteReader();
                List<Cliente> lista = new List<Cliente>();

                while (reader.Read())
                {
                    lista.Add(
                        new Cliente
                        {
                            Id = (int)reader["id_pessoa"],
                            Nome = (string)reader["nome"],
                            Cpf = (string)reader["cpf"],
                            Rg = (string)reader["rg"],
                            DatNasc = (DateTime)reader["data_nasc"],
                            Email = (string)reader["email"],
                            Telefone = (string)reader["telefone"],
                            DatCad = (DateTime)reader["dat_cad"],
                            Ativo = true
                        }
                    );
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

        public List<Cliente> MostrarPorNome(string nome)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT id_pessoa, nome, cpf, rg, data_nasc, email, telefone, dat_cad, ativo
                                    FROM clientes
                                    INNER JOIN pessoas ON id_pessoa = id
                                    WHERE nome LIKE @nome and ativo = 1
                                ";
                cmd.Parameters.AddWithValue(@"nome", string.Format("%{0}%", nome));

                SqlDataReader reader = cmd.ExecuteReader();
                List<Cliente> lista = new List<Cliente>();

                while (reader.Read())
                {
                    lista.Add(
                        new Cliente
                        {
                            Id = (int)reader["id_pessoa"],
                            Nome = (string)reader["nome"],
                            Cpf = (string)reader["cpf"],
                            Rg = (string)reader["rg"],
                            DatNasc = (DateTime)reader["data_nasc"],
                            Email = (string)reader["email"],
                            Telefone = (string)reader["telefone"],
                            DatCad = (DateTime)reader["dat_cad"],
                            Ativo = true
                        }
                    );
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
        
        public void Atualizar(int id, Cliente cliente)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE pessoas
                                    SET nome = @nome, cpf = @cpf, rg = @rg, data_nasc = @data_nasc, telefone = @telefone, email = @email
                                    WHERE id = @id
                                ";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nome", cliente.Nome);
                cmd.Parameters.AddWithValue("@cpf", cliente.Cpf);
                cmd.Parameters.AddWithValue("@rg", cliente.Rg);
                cmd.Parameters.AddWithValue("@data_nasc", cliente.DatNasc);
                cmd.Parameters.AddWithValue("@email", cliente.Email);
                cmd.Parameters.AddWithValue("@telefone", cliente.Telefone);

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
                cmd.CommandText = @"UPDATE clientes
                                    SET ativo = 0
                                    WHERE id_pessoa = @id";
                cmd.Parameters.AddWithValue("@id", id);

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
