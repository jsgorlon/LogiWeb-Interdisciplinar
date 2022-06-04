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
        

        public bool Cadastrar(Funcionario funcionario)
        {
            try
            {
                
                if(!this.verifyLogin(funcionario.Login))
                    return false; 

                cmd.Connection = connection;
                cmd.CommandText = @"SELECT id FROM pessoas WHERE cpf = @cpf;";
                cmd.Parameters.AddWithValue("@cpf", funcionario.Cpf);
                
                SqlDataReader reader = cmd.ExecuteReader();
            
                int id_pessoa = reader.HasRows ? (Int32)reader["id"] : 0; 
                
                reader.Close();
                reader.Dispose(); 
              
                if(id_pessoa > 0)
                {
                    cmd.CommandText = @"INSERT INTO pessoas (nome, cpf, rg, data_nasc, telefone, email) 
                                             VALUES (@nome, @cpf, @rg, @data_nasc, @telefone, @email);
                                             SELECT SCOPE_IDENTITY();";

                    cmd.Parameters.AddWithValue("@nome",      funcionario.Nome);
                    cmd.Parameters.AddWithValue("@cpf",       funcionario.Cpf);
                    cmd.Parameters.AddWithValue("@rg",        funcionario.Rg);
                    cmd.Parameters.AddWithValue("@data_nasc", funcionario.DatNasc);
                    cmd.Parameters.AddWithValue("@email",     funcionario.Email);
                    cmd.Parameters.AddWithValue("@telefone",  funcionario.Telefone);
                    
                    id_pessoa = (Int32) cmd.ExecuteScalar();

                    //reader.Close();
                    //reader.Dispose(); 
                }
                
                cmd.CommandText = @"INSERT INTO funcionarios (id_pessoa, id_cargo, login, senha)
                                         VALUES (@id_pessoa, @id_cargo, @login, @senha);";
                                         
                cmd.Parameters.AddWithValue("@id_pessoa", id_pessoa);
                cmd.Parameters.AddWithValue("@id_cargo",  funcionario.IdCargo);
                cmd.Parameters.AddWithValue("@login",     funcionario.Login);
                cmd.Parameters.AddWithValue("@senha",     funcionario.Senha);
                
                bool isInserted = ((Int32) cmd.ExecuteNonQuery()) > 0;
               

                return isInserted;
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }
            finally{
                Dispose(); 
            }
        }

        public List<Funcionario> Mostrar()
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT f.id_pessoa, p.nome, p.cpf, p.rg, p.data_nasc, p.email, p.telefone, c.id, c.nome, c.descricao, c.salario, p.dat_cad, f.ativo
                                    FROM funcionarios AS f
                                    JOIN pessoas AS p ON id_pessoa = p.id
                                    JOIN cargos AS c ON id_cargo = c.id
                                    WHERE f.ativo = 1;
                                ";

                SqlDataReader reader = cmd.ExecuteReader();
                List<Funcionario> lista = new List<Funcionario>();
                List<Cargo> cargos = cargoRepository.Mostrar();
                while(reader.Read())
                {
                    lista.Add(
                        new Funcionario{
                            Id = (int)reader["id_pessoa"],
                            Nome = (string)reader["nome"],
                            Cpf = (string)reader["cpf"],
                            Rg = (string)reader["rg"],
                            DatNasc = (DateTime)reader["data_nasc"],
                            Email = (string)reader["email"],
                            Telefone = (string)reader["telefone"],
                            DatCad =  (DateTime)reader["dat_cad"],
                            Ativo = true,
                            IdCargo = (short)reader["id"],
                            Cargo = cargos.FirstOrDefault<Cargo>(cargo => cargo.Id == (short)reader["id"], new Cargo())
                        }
                    );

                }
                reader.Close(); 

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

        public List<Funcionario> MostrarPorCargo(short id_cargo)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT f.id_pessoa, p.nome, p.cpf, p.rg, p.data_nasc, p.email, p.telefone, c.nome, c.descricao, c.salario, p.dat_cad, f.ativo
                                    FROM funcionarios AS f
                                    JOIN pessoas AS p ON f.id_pessoa = p.id
                                    JOIN cargos AS c ON f.id_cargo = c.id
                                    WHERE f.id_cargo = @id_cargo AND f.ativo = 1;";

                cmd.Parameters.AddWithValue("@id_cargo", id_cargo);

                SqlDataReader reader = cmd.ExecuteReader();
                List<Funcionario> lista = new List<Funcionario>();
                Cargo cargo = cargoRepository.Mostrar(id_cargo);
                while(reader.Read())
                {
                    lista.Add(
                        new Funcionario{
                            Id = (int)reader["id_pessoa"],
                            Nome = (string)reader["nome"],
                            Cpf = (string)reader["cpf"],
                            Rg = (string)reader["rg"],
                            DatNasc = (DateTime)reader["data_nasc"],
                            Email = (string)reader["email"],
                            Telefone = (string)reader["telefone"],
                            DatCad =  (DateTime)reader["dat_cad"],
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

        public List<Funcionario> MostrarPorCpf(string cpf)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT f.id_pessoa, p.nome, p.cpf, p.rg, p.data_nasc, p.email, p.telefone, c.id, c.nome, c.descricao, c.salario, p.dat_cad, f.ativo
                                    FROM funcionarios AS f
                                    JOIN pessoas AS p ON id_pessoa = p.id
                                    JOIN cargos AS c ON id_cargo = c.id
                                    WHERE cpf LIKE @cpf AND f.ativo = 1;
                                ";
                cmd.Parameters.AddWithValue(@"cpf", string.Format("%{0}%",cpf));

                SqlDataReader reader = cmd.ExecuteReader();
                List<Funcionario> lista = new List<Funcionario>();
                List<Cargo> cargos = cargoRepository.Mostrar();
                while(reader.Read())
                {
                    lista.Add(
                        new Funcionario{
                            Id = (int)reader["id_pessoa"],
                            Nome = (string)reader["nome"],
                            Cpf = (string)reader["cpf"],
                            Rg = (string)reader["rg"],
                            DatNasc = (DateTime)reader["data_nasc"],
                            Email = (string)reader["email"],
                            Telefone = (string)reader["telefone"],
                            DatCad =  (DateTime)reader["dat_cad"],
                            Ativo = true,
                            IdCargo = (short)reader["id"],
                            Cargo = cargos.FirstOrDefault<Cargo>(cargo => cargo.Id == (short)reader["id"], new Cargo())
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

        public List<Funcionario> MostrarPorNome(string nome)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT f.id_pessoa, p.nome, p.cpf, p.rg, p.data_nasc, p.email, p.telefone, c.id, c.nome, c.descricao, c.salario, p.dat_cad, f.ativo
                                    FROM funcionarios AS f
                                    JOIN pessoas AS p ON id_pessoa = p.id
                                    JOIN cargos AS c ON id_cargo = c.id
                                    WHERE p.nome LIKE @nome AND f.ativo = 1;
                                ";
                cmd.Parameters.AddWithValue(@"nome", string.Format("%{0}%",nome));
                
                SqlDataReader reader = cmd.ExecuteReader();
                List<Funcionario> lista = new List<Funcionario>();
                List<Cargo> cargos = cargoRepository.Mostrar();
                while(reader.Read())
                {
                    lista.Add(
                        new Funcionario{
                            Id = (int)reader["id_pessoa"],
                            Nome = (string)reader["nome"],
                            Cpf = (string)reader["cpf"],
                            Rg = (string)reader["rg"],
                            DatNasc = (DateTime)reader["data_nasc"],
                            Email = (string)reader["email"],
                            Telefone = (string)reader["telefone"],
                            DatCad =  (DateTime)reader["dat_cad"],
                            Ativo = true,
                            IdCargo = (short)reader["id"],
                            Cargo = cargos.FirstOrDefault<Cargo>(cargo => cargo.Id == (short)reader["id"], new Cargo())
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
                                    WHERE f.id_pessoa = @id AND f.ativo = 1;
                                ";
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    Cargo cargo = cargoRepository.Mostrar((short)reader["c.id"]);
                    return new Funcionario{
                        Id = (int)reader["id_pessoa"],
                        Nome = (string)reader["nome"],
                        Cpf = (string)reader["cpf"],
                        Rg = (string)reader["rg"],
                        DatNasc = (DateTime)reader["data_nasc"],
                        Email = (string)reader["email"],
                        Telefone = (string)reader["telefone"],
                        DatCad =  (DateTime)reader["dat_cad"],
                        Ativo = true,
                        IdCargo = (short)reader["id"],
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
                                    SET nome = @nome, cpf = @cpf, rg = @rg, data_nasc = @data_nasc, telefone = @telefone, email = @email
                                    WHERE id = @id;
                                    UPDATE funcionarios
                                    SET id_cargo = @id_cargo, login = @login, senha = @senha
                                    WHERE id_pessoa = @id;
                                ";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nome", funcionario.Nome);
                cmd.Parameters.AddWithValue("@cpf", funcionario.Cpf);
                cmd.Parameters.AddWithValue("@rg", funcionario.Rg);
                cmd.Parameters.AddWithValue("@data_nasc", funcionario.DatNasc);
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

        public bool verifyLogin(string login){

            cmd.Connection = connection;
            cmd.CommandText = @"SELECT COUNT(1) as login_free FROM funcionarios WHERE login = @login;";
            cmd.Parameters.AddWithValue("@login", login);
        
            SqlDataReader reader = cmd.ExecuteReader();
            
            bool loginIsFree = reader.HasRows; 

            return loginIsFree; 
        }
    }
}
