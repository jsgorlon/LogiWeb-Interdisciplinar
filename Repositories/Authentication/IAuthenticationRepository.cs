using logiWeb.Models;

using Microsoft.AspNetCore.Mvc; 

namespace logiWeb.Repositories
{
    public interface IAuthenticationRepository
    {
        
        bool GetUser(string Login, string Senha, HttpContext context);
        
    }
}