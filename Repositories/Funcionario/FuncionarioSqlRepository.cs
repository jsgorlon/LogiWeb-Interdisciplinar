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
        

        public string Cadastrar(Funcionario funcionario)
        {
            try
            {
                string msg = "";

                if(!this.verifyLogin(funcionario.Login))
                     msg  += "Usuário já está em uso.";

                cmd.Connection = connection;
                cmd.CommandText = @"SELECT pessoas.id FROM pessoas WHERE pessoas.cpf = @cpf;";
                cmd.Parameters.AddWithValue("@cpf", funcionario.Cpf);
                
                SqlDataReader reader = cmd.ExecuteReader();
        
                int id_pessoa = reader.Read() ? (int)reader["id"] : 0; 

                reader.Dispose();    

                if(id_pessoa == 0)
                {
                    cmd.Connection = connection;
                    cmd.CommandText = @"INSERT INTO pessoas (nome, cpf, rg, data_nasc, telefone, email) 
                                            VALUES (@nome, @cpf, @rg, @data_nasc, @telefone, @email);
                                            SELECT SCOPE_IDENTITY();";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@nome",      funcionario.Nome.ToUpper());
                    cmd.Parameters.AddWithValue("@cpf",       funcionario.Cpf);
                    cmd.Parameters.AddWithValue("@data_nasc", funcionario.DatNasc);
                    cmd.Parameters.AddWithValue("@rg",       funcionario.Rg       ?? DBNull.Value.ToString());
                    cmd.Parameters.AddWithValue("@email",    funcionario.Email    ?? DBNull.Value.ToString());
                    cmd.Parameters.AddWithValue("@telefone", funcionario.Telefone ?? DBNull.Value.ToString());
                
                    id_pessoa = (int)cmd.ExecuteScalar();
                }
                else {
                    cmd.Connection = connection; 
                    cmd.CommandText = @"SELECT id_pessoa FROM funcionarios WHERE funcionarios.id_pessoa = @id_pessoa";
                    cmd.Parameters.Clear(); 
                    cmd.Parameters.AddWithValue("@id_pessoa", id_pessoa);

                    SqlDataReader func_reader = cmd.ExecuteReader();
            
                    if(func_reader.HasRows)
                        msg += "Este CPF já está associado a uma pessoa.";

                    func_reader.Close();
                }

                if(msg != "")
                    return "alert_error('"+msg+"');"; 

                cmd.Connection = connection; 
                cmd.CommandText = @"INSERT INTO funcionarios (id_pessoa, id_cargo, login, senha)
                                        VALUES (@id_pessoa, @id_cargo, @login, @senha);";
                cmd.Parameters.Clear();                        
                cmd.Parameters.AddWithValue("@id_pessoa", id_pessoa);
                cmd.Parameters.AddWithValue("@id_cargo",  funcionario.IdCargo);
                cmd.Parameters.AddWithValue("@login",     funcionario.Login);
                cmd.Parameters.AddWithValue("@senha",     funcionario.Senha);
                
                bool isInserted = ((int)cmd.ExecuteNonQuery()) > 0;
                
                cmd.Dispose();

                return "alert_success('Funcionário cadastrado com sucesso!');";
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }
            finally{
                Dispose(); 
            }
        }

        public List<Funcionario> Mostrar(string? nome, int? id_cargo, int? status)
        {             
            try
            {
                string filters = "";
                bool cargo_is_valid = id_cargo == 1 || id_cargo == 2 || id_cargo == 3; 
                bool filter_status = status == 1 || status == 0; 

                if(!String.IsNullOrEmpty(nome))
                    filters += " AND pessoas.nome LIKE @nome";
                
                if(cargo_is_valid)
                    filters += " AND funcionarios.id_cargo = @id_cargo";
                
                if(filter_status)
                    filters += " AND funcionarios.ativo = @status";

                cmd.Connection = connection;
                cmd.CommandText = @"SELECT * 
                                      FROM pessoas, 
                                           funcionarios
                                     WHERE funcionarios.id_pessoa = pessoas.id
                                           "+filters+" ORDER BY pessoas.nome";
                cmd.Parameters.Clear();
                
                if(!String.IsNullOrEmpty(nome))
                    cmd.Parameters.AddWithValue("@nome", "%"+nome.ToUpper()+"%"); 
                
                if(cargo_is_valid)
                    cmd.Parameters.AddWithValue("@id_cargo", id_cargo); 
               
                if(filter_status)
                    cmd.Parameters.AddWithValue("@status", status); 


                SqlDataReader reader = cmd.ExecuteReader();
                List<Funcionario> lista = new List<Funcionario>();
                List<Cargo> cargos = cargoRepository.Mostrar();

                while(reader.Read())
                {
                   lista.Add(new Funcionario{
                                                Id       = (int)reader["id_pessoa"],
                                                Nome     = (string)reader["nome"],
                                                Cpf      = (string)reader["cpf"],
                                                Rg       = (string)reader["rg"],
                                                DatNasc  = (DateTime)reader["data_nasc"],
                                                Email    = (string)reader["email"],
                                                Telefone = (string)reader["telefone"],
                                                DatCad   = (DateTime)reader["dat_cad"],
                                                Ativo    =  (bool)reader["ativo"],
                                                IdCargo  = (short)reader["id_cargo"],
                                                Cargo    = cargos.FirstOrDefault<Cargo>(cargo => cargo.Id == (short)reader["id_cargo"], new Cargo())
                                            });

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

        public string Atualizar(int id, Funcionario funcionario) 
        {
            try
            {
                if(!this.verifyLogin(funcionario.Login, id))
                     return "alert_error('Usuário já está em uso.');";

                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE pessoas
                                       SET nome = @nome, 
                                           cpf = @cpf, 
                                           rg = @rg, 
                                           data_nasc = @data_nasc, 
                                           telefone = @telefone, 
                                           email = @email
                                     WHERE id = @id;
                                    UPDATE funcionarios
                                       SET id_cargo = @id_cargo, login = @login, senha = @senha
                                     WHERE id_pessoa = @id;
                                ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nome", funcionario.Nome);
                cmd.Parameters.AddWithValue("@cpf", funcionario.Cpf);
                cmd.Parameters.AddWithValue("@data_nasc", funcionario.DatNasc);
                cmd.Parameters.AddWithValue("@rg",       funcionario.Rg       ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@email",    funcionario.Email    ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@telefone", funcionario.Telefone ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@id_cargo", funcionario.IdCargo);
                cmd.Parameters.AddWithValue("@login", funcionario.Login);
                cmd.Parameters.AddWithValue("@senha", funcionario.Senha);
                cmd.ExecuteNonQuery();

                return "alert_success('Dados atualizados com sucesso!');";
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

        public void AlterarStatus(int id, int status)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE funcionarios
                                    SET ativo = @status
                                    WHERE id_pessoa = @id_pessoa
                                ";
                cmd.Parameters.Clear(); 
                cmd.Parameters.AddWithValue("@id_pessoa", id);
                cmd.Parameters.AddWithValue("@status", status);
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

        public bool verifyLogin(string login, int? id = null){
            
            string filter = "";

            if(id != null)
                filter = " AND id_pessoa <> @id_pessoa"; 

            cmd.Connection = connection;
            cmd.CommandText = @"SELECT id_pessoa FROM funcionarios WHERE login = @login "+filter;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@login", login);

            if(id != null)
                cmd.Parameters.AddWithValue("@id_pessoa", id);

            SqlDataReader reader = cmd.ExecuteReader();
            
            bool loginIsFree = reader.Read() ? false : true; 
            reader.Close();
           
            return loginIsFree; 
        }
    }
}
