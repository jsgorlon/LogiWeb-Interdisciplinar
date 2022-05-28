using System.Data.SqlClient;
using logiWeb.Models;

namespace logiWeb.Repositories
{
    public class ClienteSqlRepository : DBContext, IClienteRepository
    {
        ///TODO Escreve Querys do Banco de Dados
        private SqlCommand cmd = new SqlCommand();
        public void Cadastrar(Cliente cliente)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT id FROM pessoas WHERE cpf = @cpf";
                cmd.Parameters.AddWithValue("@cpf", cliente.Cpf);
                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    cmd.CommandText = @"INSERT INTO pessoas (nome, cpf, rg, data_nasc, telefone, email) 
                                            VALUES (@nome, @cpf, @rg, @data_nasc, @telefone, @email);
                                        INSERT INTO clientes (id_pessoa) VALUES (SCOPE_IDENTITY());
                                    ";
                    cmd.Parameters.AddWithValue("@nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@cpf", cliente.Cpf);
                    cmd.Parameters.AddWithValue("@rg", cliente.Rg);
                    cmd.Parameters.AddWithValue("@dataNasc", cliente.DatNasc);
                    cmd.Parameters.AddWithValue("@email", cliente.Email);
                    cmd.Parameters.AddWithValue("@telefone", cliente.Telefone);

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = @"SELECT id FROM pessoas WHERE cpf = @cpf";
                    cmd.Parameters.AddWithValue("@cpf", cliente.Cpf);

                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int id_pessoa = (int)reader["id"];
                        cmd.CommandText = @"INSERT INTO clientes (id_pessoa) VALUES (@id_pessoa)";
                        cmd.Parameters.AddWithValue("@id_pessoa", id_pessoa);
                        cmd.ExecuteNonQuery();
                    }
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

        public List<Cliente> Mostrar()
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT id_pessoa, nome, cpf, rg, data_nasc, email, telefone, dat_cad, ativo
                                    FROM clientes
                                    INNER JOIN pessoas ON id_pessoa = id
                                    WHERE ativo = 1
                                ";

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
                            DatNasc = DateOnly.FromDateTime((DateTime)reader["data_nasc"]),
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
                                    WHERE id_pessoa = @id AND ativo = 1
                                ";

                cmd.Parameters.AddWithValue(@"id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    return new Cliente 
                    {
                        Id = (int)reader["id_pessoa"],
                        Nome = (string)reader["nome"],
                        Cpf = (string)reader["cpf"],
                        Rg = (string)reader["rg"],
                        DatNasc = DateOnly.FromDateTime((DateTime)reader["data_nasc"]),
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

        public Cliente MostrarPorCpf(string cpf)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT id_pessoa, nome, cpf, rg, data_nasc, email, telefone, dat_cad, ativo
                                    FROM clientes
                                    INNER JOIN pessoas ON id_pessoa = id
                                    WHERE cpf = @cpf
                                ";
                cmd.Parameters.AddWithValue(@"cpf", cpf);

                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    return new Cliente 
                    {
                        Id = (int)reader["id_pessoa"],
                        Nome = (string)reader["nome"],
                        Cpf = (string)reader["cpf"],
                        Rg = (string)reader["rg"],
                        DatNasc = DateOnly.FromDateTime((DateTime)reader["data_nasc"]),
                        Email = (string)reader["email"],
                        Telefone = (string)reader["telefone"],
                        DatCad = (DateTime)reader["dat_cad"],
                        Ativo = true
                    };
                }
                return null;
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
                                    SET nome = @nome, cpf = @cpf, rg = @rg, data_nasc = @dataNasc, telefone = @telefone, email = @email
                                    WHERE id = @id
                                ";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nome", cliente.Nome);
                cmd.Parameters.AddWithValue("@cpf", cliente.Cpf);
                cmd.Parameters.AddWithValue("@rg", cliente.Rg);
                cmd.Parameters.AddWithValue("@dataNasc", cliente.DatNasc);
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
                                    WHERE id_pessoa = id
                                ";
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
