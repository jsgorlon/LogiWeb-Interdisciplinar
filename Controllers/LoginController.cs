
namespace logiWeb.Controllers; 

using Microsoft.AspNetCore.Mvc; 

public class LoginController : Controller 
{
    [HttpGet]
    public ActionResult Index() 
    {
      return View(); 
    }

    [HttpPost]
    public string Autenticar(string Login, string Senha)
    { 
      
      
      return Login;
    }
}