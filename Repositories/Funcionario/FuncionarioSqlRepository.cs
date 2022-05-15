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
            throw new NotImplementedException();
        }

        public List<Funcionario> Mostrar()
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT f.id_pessoa, p.nome, p.cpf, p.rg, p.data_nasc, p.email, p.telefone, c.id, c.nome, c.descricao, c.salario, p.data_cad, f.ativo
                                    FROM funcionarios AS f
                                    JOIN pessoas AS p ON f.id_pessoa = p.id
                                    JOIN cargos AS c ON f.id_cargo = c.id";

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
                            DatNasc = (DateOnly)reader["p.data_nasc"],
                            Email = (string)reader["p.email"],
                            Telefone = (string)reader["p.telefone"],
                            DatCad =  (DateTime)reader["p.data_cad"],
                            Ativo = (bool)reader["f.ativo"],
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
                cmd.CommandText = @"SELECT f.id_pessoa, p.nome, p.cpf, p.rg, p.data_nasc, p.email, p.telefone, c.nome, c.descricao, c.salario, p.data_cad, f.ativo
                                    FROM funcionarios AS f
                                    JOIN pessoas AS p ON f.id_pessoa = p.id
                                    JOIN cargos AS c ON f.id_cargo = c.id
                                    WHERE f.id_cargo = @id_cargo";

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
                            DatNasc = (DateOnly)reader["p.data_nasc"],
                            Email = (string)reader["p.email"],
                            Telefone = (string)reader["p.telefone"],
                            DatCad =  (DateTime)reader["p.data_cad"],
                            Ativo = (bool)reader["f.ativo"],
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
                cmd.CommandText = @"SELECT f.id_pessoa, p.nome, p.cpf, p.rg, p.data_nasc, p.email, p.telefone, c.id, c.nome, c.descricao, c.salario, p.data_cad, f.ativo
                                    FROM funcionarios AS f
                                    JOIN pessoas AS p ON f.id_pessoa = p.id
                                    JOIN cargos AS c ON f.id_cargo = c.id
                                    WHERE f.id_pessoa = @id";
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
                        DatNasc = (DateOnly)reader["p.data_nasc"],
                        Email = (string)reader["p.email"],
                        Telefone = (string)reader["p.telefone"],
                        DatCad =  (DateTime)reader["p.data_cad"],
                        Ativo = (bool)reader["f.ativo"],
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
                cmd.CommandText = @"";

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
                cmd.CommandText = "";
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