using System.Data.SqlClient;
using logiWeb.Models;
using logiWeb.Helpers; 

namespace logiWeb.Repositories
{
    public class FuncionarioSqlRepository : DBContext, IFuncionarioRepository
    {
       
        private ICargoRepository cargoRepository;
        private AjaxResponse response = new AjaxResponse(); 

        public FuncionarioSqlRepository(ICargoRepository cargoRepository)
        {
          this.cargoRepository = cargoRepository;
          
        }
        

        public string Cadastrar(Funcionario funcionario)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
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
                
                    id_pessoa = Convert.ToInt32((decimal)cmd.ExecuteScalar());
                }
                else {
                    cmd.Connection = connection; 
                    cmd.CommandText = @"SELECT funcionarios.id_pessoa 
                                          FROM funcionarios 
                                         WHERE funcionarios.id_pessoa = @id_pessoa";
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

                return "alert_success('Funcionário cadastrado com sucesso!');dialog.close();";
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }
            finally{
                Dispose(); 
            }
        }

        public AjaxResponse Mostrar(string? nome, int? id_cargo, int? status)
        {             
            try
            {
                 SqlCommand cmd = new SqlCommand();

                string filters = "";
         
                if(nome != null)
                    filters += " AND pessoas.nome LIKE @nome";
                
                if(id_cargo != null)
                    filters += " AND funcionarios.id_cargo = @id_cargo";
                
                if(status != null)
                    filters += " AND funcionarios.ativo = @status";

                cmd.Connection = connection;
                cmd.CommandText = @"SELECT * 
                                      FROM pessoas, 
                                           funcionarios
                                     WHERE funcionarios.id_pessoa = pessoas.id
                                           "+filters+" ORDER BY pessoas.nome";
                cmd.Parameters.Clear();
                
                if(nome != null)
                    cmd.Parameters.AddWithValue("@nome", "%"+nome.ToUpper()+"%"); 
                
                 if(id_cargo != null)
                    cmd.Parameters.AddWithValue("@id_cargo", id_cargo); 
               
                if(status != null)
                    cmd.Parameters.AddWithValue("@status", status); 


                SqlDataReader reader = cmd.ExecuteReader();
                List<Funcionario> funcionarios = new List<Funcionario>();
                List<Cargo> cargos = cargoRepository.Mostrar();

                while(reader.Read())
                {
                   funcionarios.Add(new Funcionario{
                                                Id       = (int)reader["id_pessoa"],
                                                Nome     = (string)reader["nome"],
                                                Cpf      = (string)reader["cpf"],
                                                Rg       = reader["rg"] is System.DBNull ? string.Empty : (string)reader["rg"],
                                                DatNasc  = (DateTime)reader["data_nasc"],
                                                Email    = (string)reader["email"],
                                                Telefone = reader["telefone"] is System.DBNull ? string.Empty : (string)reader["telefone"],
                                                Login =  (string)reader["login"],
                                                DatCad   = (DateTime)reader["dat_cad"],
                                                Ativo    =  (bool)reader["ativo"],
                                                IdCargo  = (short)reader["id_cargo"],
                                                Cargo    = cargos.FirstOrDefault<Cargo>(cargo => cargo.Id == (short)reader["id_cargo"], new Cargo())
                                            });

                }
                reader.Close(); 

                response.Item.Add("funcionarios", funcionarios); 
                return response;
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
                 SqlCommand cmd = new SqlCommand();

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
                                     WHERE id = @id;";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nome",     funcionario.Nome.ToUpper());
                cmd.Parameters.AddWithValue("@cpf",      funcionario.Cpf);
                cmd.Parameters.AddWithValue("@data_nasc",funcionario.DatNasc);
                cmd.Parameters.AddWithValue("@rg",       funcionario.Rg       ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@email",    funcionario.Email    ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@telefone", funcionario.Telefone ?? DBNull.Value.ToString());
                
                cmd.ExecuteNonQuery();


                cmd.CommandText = @"UPDATE funcionarios
                                       SET id_cargo = @id_cargo, login = @login, senha = @senha
                                     WHERE id_pessoa = @id;";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@id_cargo", funcionario.IdCargo);
                cmd.Parameters.AddWithValue("@login",    funcionario.Login);
                cmd.Parameters.AddWithValue("@senha",    funcionario.Senha);
                cmd.ExecuteNonQuery();

                return "alert_success('Dados atualizados com sucesso!');dialog.close();";
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
                 SqlCommand cmd = new SqlCommand();
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
             SqlCommand cmd = new SqlCommand();
             
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
