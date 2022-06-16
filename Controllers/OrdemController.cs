using logiWeb.Models;
using logiWeb.Helpers; 
using logiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace logiWeb.Controllers; 
public class OrdemController : Controller
{
    private IOrdemRepository repository;
    private IClienteRepository clienteRepository;

    public OrdemController(IOrdemRepository repository, IClienteRepository clienteRepository)
    {
        this.repository = repository;
        this.clienteRepository = clienteRepository;
    }
     public ActionResult Index()
     {
            return View();
     }

    [HttpGet]
    public JsonResult Ordem(string? nome)
    {
        List<Ordem> ordens = this.repository.MostrarOrdens();
        return Json(ordens);
    }


    [HttpPost]
    public AjaxResponse Cadastrar(Ordem ordem, Endereco endereco)
    {
        return this.repository.Cadastrar(ordem, endereco);
    }

    
    public ActionResult Excluir(int id)
    {
        this.repository.Excluir(id);
        return RedirectToAction("Mostrar");
    }

}