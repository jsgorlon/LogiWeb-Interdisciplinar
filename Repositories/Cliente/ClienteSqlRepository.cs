using System.Data.SqlClient;
using logiWeb.Models;

namespace logiWeb.Repositories

{
    public class ClienteSqlRepository : DBContext, IClienteFisicoRepository, IClienteJuridicoRepository
    {
        private SqlCommand cmd = new SqlCommand();
    ///Métodos para Cliente Fisico
        public void CadastrarClienteFisico(ClienteFisico cliente)
        {
            cmd.Connection = connection;
            cmd.CommandText = @"";

            cmd.ExecuteNonQuery();
        }

        public List<ClienteFisico> MostrarClienteFisico()
        {
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT c.id_pessoa, p.nome, p.email, p.data_cad, pf.cpf, pf.rg, pf.data_nasc
                                FROM clientes AS c
                                INNER JOIN pessoa_fisica AS pf ON c.id_pessoa = pf.id_pessoa
                                INNER JOIN pessoas AS p ON c.id_pessoa = p.id
            ";

            SqlDataReader reader = cmd.ExecuteReader();

            List<ClienteFisico> lista = new List<ClienteFisico>();

            while (reader.Read())
            {
                lista.Add(
                    new ClienteFisico{
                        Id = (int)reader["id_pessoa"],
                        Nome = (string)reader["nome"],
                        Email = (string)reader["email"],
                        DatCad = (DateTime)reader["dat_cad"],
                        Cpf = (string)reader["cpf"],
                        Rg = (string)reader["rg"],
                        DatNasc = (DateOnly)reader["data_nasc"],
                    }
                );
            }
            return lista;
        }

        public ClienteFisico MostrarClienteFisico(int id)
        {
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT c.id_pessoa, p.nome, p.email, p.data_cad, pf.cpf, pf.rg, pf.data_nasc
                                FROM clientes AS c
                                INNER JOIN pessoa_fisica AS pf ON c.id_pessoa = pf.id_pessoa
                                INNER JOIN pessoas AS p ON c.id_pessoa = p.id
                                WHERE c.id_pessoa = @id
            ";

            cmd.Parameters.AddWithValue(@"id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                ClienteFisico cliente = new ClienteFisico{
                    Id = (int)reader["id_pessoa"],
                    Nome = (string)reader["nome"],
                    Email = (string)reader["email"],
                    DatCad = (DateTime)reader["dat_cad"],
                    Cpf = (string)reader["cpf"],
                    Rg = (string)reader["rg"],
                    DatNasc = (DateOnly)reader["data_nasc"],
                };

                cmd.CommandText = @"SELECT id_pessoa, ddd, nr_telefone
                                FROM telefones
                                WHERE id_pessoa = @id
                ";
                cmd.Parameters.AddWithValue(@"id", id);
                reader = cmd.ExecuteReader();
                List<Telefone> lista = new List<Telefone>();
                while (reader.Read())
                {
                    lista.Add(
                        new Telefone{
                            DDD = (string)reader["ddd"],
                            Numero = (string)reader["nr_telefone"],
                        }
                    );
                }
            }
            return null;
        }
        
        public void AtualizarClienteFisico(int id, ClienteFisico cliente)
        {
            cmd.Connection = connection;
            cmd.CommandText = @"UPDATE c.id_pessoa, p.nome, p.email, p.data_cad, pf.cpf, pf.rg, pf.data_nasc";
        }

        public void ExcluirClienteFisico(int id)
        {
            cmd.Connection = connection;
            cmd.CommandText = @"";
        }

    /// Metodos para Cliente Juridico
        public void CadastrarClienteJuridico(ClienteJuridico cliente)
        {
            cmd.Connection = connection;
            cmd.CommandText = @"";

            cmd.ExecuteNonQuery();
        }

        public List<ClienteJuridico> MostrarClienteJuridico()
        {
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT c.id_pessoa, p.nome, p.email, p.data_cad, pj.cnpj, pj.razao_social
                                FROM clientes AS c
                                INNER JOIN pessoa_juridica AS pj ON c.id_pessoa = pj.id_pessoa
                                INNER JOIN pessoas AS p ON c.id_pessoa = p.id
            ";

            SqlDataReader reader = cmd.ExecuteReader();

            List<ClienteJuridico> lista = new List<ClienteJuridico>();

            while (reader.Read())
            {
                lista.Add(
                    new ClienteJuridico{
                        Id = (int)reader["id_pessoa"],
                        Nome = (string)reader["nome"],
                        Email = (string)reader["email"],
                        DatCad = (DateTime)reader["dat_cad"],
                        Cnpj = (string)reader["cnpj"],
                        RazaoSocial = (string)reader["razao_social"],
                    }
                );
            }
            return lista;
        }

        public ClienteJuridico MostrarClienteJuridico(int id)
        {
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT c.id_pessoa, p.nome, p.email, p.data_cad, pj.cnpj, pj.razao_social
                                FROM clientes AS c
                                INNER JOIN pessoa_juridica AS pj ON c.id_pessoa = pj.id_pessoa
                                INNER JOIN pessoas AS p ON c.id_pessoa = p.id
                                WHERE c.id_pessoa = @id
            ";

            cmd.Parameters.AddWithValue(@"id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                ClienteJuridico cliente = new ClienteJuridico{
                    Id = (int)reader["id_pessoa"],
                    Nome = (string)reader["nome"],
                    Email = (string)reader["email"],
                    DatCad = (DateTime)reader["dat_cad"],
                    Cnpj = (string)reader["cpf"],
                    RazaoSocial = (string)reader["rg"],
                };

                cmd.CommandText = @"SELECT id_pessoa, ddd, nr_telefone
                                FROM telefones
                                WHERE id_pessoa = @id
                ";
                cmd.Parameters.AddWithValue(@"id", id);
                reader = cmd.ExecuteReader();
                List<Telefone> lista = new List<Telefone>();
                while (reader.Read())
                {
                    lista.Add(
                        new Telefone{
                            DDD = (string)reader["ddd"],
                            Numero = (string)reader["nr_telefone"],
                        }
                    );
                }
            }
            return null;
        }
        
        public void AtualizarClienteJuridico(int id, ClienteJuridico cliente)
        {
            cmd.Connection = connection;
            cmd.CommandText = @"UPDATE c.id_pessoa, p.nome, p.email, p.data_cad, pj.cnpj, pj.razao_social";
        }

        public void ExcluirClienteJuridico(int id)
        {
            cmd.Connection = connection;
            cmd.CommandText = @"";
        }
    }
}