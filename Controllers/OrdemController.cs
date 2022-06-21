using logiWeb.Models;
using logiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace logiWeb.Controllers; 
public class OrdemController : Controller
{
    private IOrdemRepository repository;

    public OrdemController(IOrdemRepository repository)
    {
        this.repository = repository;
    }
     public ActionResult Index()
     {
        ViewBag.IdFuncionario =  HttpContext.Session.GetString("id_funcionario"); 
        return View();
     }

   
    [HttpPost]
    public JsonResult Cadastrar(Ordem ordem, Endereco endereco)
    {
       return Json(this.repository.Cadastrar(ordem, endereco));
    }

    public JsonResult Todas(int? id_funcionario,string? nome, int? status){
        var ordens = this.repository.MostrarOrdens(id_funcionario, nome, status); 
        
        return Json(ordens);
    }

    public JsonResult AlterarStatus(int id, int status)
    {
       return Json(this.repository.AlterarStatus(id, status));
    }

    public JsonResult getById(int id){

        return Json(this.repository.GetById(id));
    }
}