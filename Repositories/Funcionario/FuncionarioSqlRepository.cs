using System.Data.SqlClient;
using logiWeb.Models;

namespace logiWeb.Repositories
{
    public class FuncionarioSqlRepository : DBContext, IFuncionarioRepository
    {
        private SqlCommand cmd = new SqlCommand();
        private ICargoRepository cargoRepository;

        public FuncionarioSqlRepository(ICargoRepository cargoRepository)
        {
            this.cargoRepository = cargoRepository;
        }
        

        public void Cadastrar(Funcionario funcionario)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT id FROM pessoas WHERE cpf = @cpf";
                cmd.Parameters.AddWithValue("@cpf", funcionario.Cpf);
                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    cmd.CommandText = @"INSERT INTO pessoas (nome, cpf, rg, data_nasc, telefone, email) 
                                            VALUES (@nome, @cpf, @rg, @data_nasc, @telefone, @email);
                                        INSERT INTO funcionarios (id_pessoa, id_cargo, login, senha)
                                            VALUES (SCOPE_IDENTITY(), @id_cargo, @login, @senha);
                                    ";
                    cmd.Parameters.AddWithValue("@nome", funcionario.Nome);
                    cmd.Parameters.AddWithValue("@cpf", funcionario.Cpf);
                    cmd.Parameters.AddWithValue("@rg", funcionario.Rg);
                    cmd.Parameters.AddWithValue("@dataNasc", funcionario.DatNasc);
                    cmd.Parameters.AddWithValue("@email", funcionario.Email);
                    cmd.Parameters.AddWithValue("@telefone", funcionario.Telefone);
                    cmd.Parameters.AddWithValue("@id_cargo", funcionario.IdCargo);
                    cmd.Parameters.AddWithValue("@login", funcionario.Login);
                    cmd.Parameters.AddWithValue("@senha", funcionario.Senha);

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = @"SELECT id FROM pessoas WHERE cpf = @cpf";
                    cmd.Parameters.AddWithValue("@cpf", funcionario.Cpf);

                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int id_pessoa = (int)reader["id"];
                        cmd.CommandText = @"INSERT INTO funcionarios (id_pessoa, id_cargo, login, senha)
                                            VALUES (@id_pessoa, @id_cargo, @login, @senha)";
                        cmd.Parameters.AddWithValue("@id_pessoa", id_pessoa);
                        cmd.Parameters.AddWithValue("@id_cargo", funcionario.IdCargo);
                        cmd.Parameters.AddWithValue("@login", funcionario.Login);
                        cmd.Parameters.AddWithValue("@senha", funcionario.Senha);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Funcionario> Mostrar()
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT f.id_pessoa, p.nome, p.cpf, p.rg, p.data_nasc, p.email, p.telefone, c.id, c.nome, c.descricao, c.salario, p.dat_cad, f.ativo
                                    FROM funcionarios AS f
                                    JOIN pessoas AS p ON f.id_pessoa = p.id
                                    JOIN cargos AS c ON f.id_cargo = c.id
                                    WHERE f.ativo = 1
                                ";

                SqlDataReader reader = cmd.ExecuteReader();
                List<Funcionario> lista = new List<Funcionario>();
                List<Cargo> cargos = cargoRepository.Mostrar();
                while(reader.Read())
                {
                    lista.Add(
                        new Funcionario{
                            Id = (int)reader["f.id_pessoa"],
                            Nome = (string)reader["p.nome"],
                            Cpf = (string)reader["p.cpf"],
                            Rg = (string)reader["p.rg"],
                            DatNasc = (DateTime)reader["p.data_nasc"],
                            Email = (string)reader["p.email"],
                            Telefone = (string)reader["p.telefone"],
                            DatCad =  (DateTime)reader["p.dat_cad"],
                            Ativo = true,
                            IdCargo = (int)reader["c.id"],
                            Cargo = cargos.FirstOrDefault<Cargo>(cargo => cargo.Id == (int)reader["c.id"], new Cargo())
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

        public List<Funcionario> MostrarPorCargo(int id_cargo)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT f.id_pessoa, p.nome, p.cpf, p.rg, p.data_nasc, p.email, p.telefone, c.nome, c.descricao, c.salario, p.dat_cad, f.ativo
                                    FROM funcionarios AS f
                                    JOIN pessoas AS p ON f.id_pessoa = p.id
                                    JOIN cargos AS c ON f.id_cargo = c.id
                                    WHERE f.id_cargo = @id_cargo AND f.ativo = 1";

                cmd.Parameters.AddWithValue("@id_cargo", id_cargo);

                SqlDataReader reader = cmd.ExecuteReader();
                List<Funcionario> lista = new List<Funcionario>();
                Cargo cargo = cargoRepository.Mostrar(id_cargo);
                while(reader.Read())
                {
                    lista.Add(
                        new Funcionario{
                            Id = (int)reader["f.id_pessoa"],
                            Nome = (string)reader["p.nome"],
                            Cpf = (string)reader["p.cpf"],
                            Rg = (string)reader["p.rg"],
                            DatNasc = (DateTime)reader["p.data_nasc"],
                            Email = (string)reader["p.email"],
                            Telefone = (string)reader["p.telefone"],
                            DatCad =  (DateTime)reader["p.dat_cad"],
                            Ativo = true,
                            IdCargo = id_cargo,
                            Cargo = cargo
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

        public Funcionario Mostrar(int id)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT f.id_pessoa, p.nome, p.cpf, p.rg, p.data_nasc, p.email, p.telefone, c.id, c.nome, c.descricao, c.salario, p.dat_cad, f.ativo
                                    FROM funcionarios AS f
                                    JOIN pessoas AS p ON f.id_pessoa = p.id
                                    JOIN cargos AS c ON f.id_cargo = c.id
                                    WHERE f.id_pessoa = @id and f.ativo = 1
                                ";
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    Cargo cargo = cargoRepository.Mostrar((int)reader["c.id"]);
                    return new Funcionario{
                        Id = (int)reader["f.id_pessoa"],
                        Nome = (string)reader["p.nome"],
                        Cpf = (string)reader["p.cpf"],
                        Rg = (string)reader["p.rg"],
                        DatNasc = (DateTime)reader["p.data_nasc"],
                        Email = (string)reader["p.email"],
                        Telefone = (string)reader["p.telefone"],
                        DatCad =  (DateTime)reader["p.dat_cad"],
                        Ativo = true,
                        IdCargo = (int)reader["c.id"],
                        Cargo = cargo
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

        public void Atualizar(int id, Funcionario funcionario) 
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE pessoas
                                    SET nome = @nome, cpf = @cpf, rg = @rg, data_nasc = @dataNasc, telefone = @telefone, email = @email
                                    WHERE id = @id;
                                    UPDATE funcionarios
                                    Set id_cargo = @id_cargo, login = @login, senha = @senha
                                    Where id_pessoa = @id;
                                ";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nome", funcionario.Nome);
                cmd.Parameters.AddWithValue("@cpf", funcionario.Cpf);
                cmd.Parameters.AddWithValue("@rg", funcionario.Rg);
                cmd.Parameters.AddWithValue("@dataNasc", funcionario.DatNasc);
                cmd.Parameters.AddWithValue("@email", funcionario.Email);
                cmd.Parameters.AddWithValue("@telefone", funcionario.Telefone);
                cmd.Parameters.AddWithValue("@id_cargo", funcionario.IdCargo);
                cmd.Parameters.AddWithValue("@login", funcionario.Login);
                cmd.Parameters.AddWithValue("@senha", funcionario.Senha);
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
                cmd.CommandText = @"UPDATE funcionarios
                                    SET ativo = 0
                                    WHERE id_pessoa = @id
                                ";
                cmd.Parameters.AddWithValue("@id_pessoa", id);
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
