using logiWeb.Models;

namespace logiWeb.Repositories
{
    public interface IAuthenticationRepository
    {
        
        bool GetUser(string Login, string Senha);
        
    }
}