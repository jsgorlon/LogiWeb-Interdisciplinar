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
                cmd.CommandText = @"";

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
                cmd.CommandText = @"SELECT c.id_pessoa, p.nome, p.email, p.data_cad, pf.cpf, pf.rg, pf.data_nasc
                                    FROM clientes AS c
                                    INNER JOIN pessoa_fisica AS pf ON c.id_pessoa = pf.id_pessoa
                                    INNER JOIN pessoas AS p ON c.id_pessoa = p.id
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
                            DatNasc = (DateOnly)reader["data_nasc"],
                            Email = (string)reader["email"],
                            Telefone = (string)reader["telefone"],
                            DatCad = (DateTime)reader["dat_cad"]
                        }
                    );
                }
                return lista;
            }
            catch(Exception ex)
            {
                return null;
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
                cmd.CommandText = @"SELECT p.id, p.nome, p.cpf, p.rg, p.data_nasc, p.email, p.data_cad, c.ativo
                                    FROM pessoa AS p
                                    INNER JOIN cliente AS c ON c.id_pessoa = p.id
                                    WHERE p.id = @id
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
                        DatNasc = (DateOnly)reader["data_nasc"],
                        Email = (string)reader["email"],
                        Telefone = (string)reader["telefone"],
                        DatCad = (DateTime)reader["dat_cad"],
                    };
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                return null;
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
                cmd.CommandText = @"SELECT p.id, p.nome, p.cpf, p.rg, p.data_nasc, p.email, p.data_cad, c.ativo
                                    FROM pessoa AS p
                                    INNER JOIN cliente AS c ON c.id_pessoa = p.id
                                    WHERE p.cpf = @cpf
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
                        DatNasc = (DateOnly)reader["data_nasc"],
                        Email = (string)reader["email"],
                        Telefone = (string)reader["telefone"],
                        DatCad = (DateTime)reader["dat_cad"],
                    };
                }
                return null;
            }
            catch(Exception e)
            {
                return null;
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
                cmd.CommandText = @"UPDATE c.id_pessoa, p.nome, p.email, p.data_cad, pf.cpf, pf.rg, pf.data_nasc";

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
                cmd.CommandText = @"";

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                Dispose();
            }
        }
    }
}