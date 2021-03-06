using System.Data.SqlClient;
using logiWeb.Models;
using Microsoft.AspNetCore.Mvc;


namespace logiWeb.Repositories
{
    public class IAuthenticationSqlRepository: DBContext, IAuthenticationRepository
    {
     
      
        public bool GetUser(string Login, string Senha, HttpContext context)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT * 
                                      FROM funcionarios 
                                     WHERE login = @login";
                cmd.Parameters.Clear(); 
                cmd.Parameters.AddWithValue("@login", Login);
                SqlDataReader reader = cmd.ExecuteReader();
                
                if(reader.Read()){

                    string senha = (string)reader["senha"];

                    int id_funcionario = (int)reader["id_pessoa"]; 

                    context.Session.SetString("id_funcionario", id_funcionario.ToString());

                    bool loginIsValid = (senha == Senha); 

                    return loginIsValid; 
                }

                return false; 
            }
            catch (Exception ex)
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