using logiWeb.Models;
using logiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace logiWeb.Controllers; 

public class LoginController : Controller 
{
    private IAuthenticationRepository repository;

     public LoginController(IAuthenticationRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public ActionResult Index() 
    {

      if(HttpContext.Session.Get("Auth") != null)
        HttpContext.Response.Redirect("/ordem");

      return View(); 
    }

    [HttpPost]
    public string Autenticar(string Login, string Senha)
    { 
      
      if(repository.GetUser(Login, Senha, HttpContext))
      {
        HttpContext.Session.SetString("Auth", "True"); 
        return "window.location.href = '/ordem';";
      } 
      
      return "$('#msg_usuario_senha_invalidos').css('visibility','visible');";
    }

    public void Logout(){

       HttpContext.Session.Clear(); 
       HttpContext.Response.Redirect("/");
    }
}