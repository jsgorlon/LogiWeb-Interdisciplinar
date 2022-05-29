using logiWeb.Models;
using logiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

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

    /*
    [HttpPost]
    public JsonResult Index()
    {
        List<Ordem> ordem = this.repository.MostrarOrdens();
        return Json(ordem);
    } */

    [HttpGet]
    public ActionResult Cadastrar()
    {
        ViewBag.Clientes = clienteRepository.Mostrar();
        return View();
    }

    [HttpPost]
    public ActionResult Cadastrar(Ordem ordem)
    {
        this.repository.Cadastrar(ordem);
        return RedirectToAction("Mostrar");
    }

    
    public ActionResult Excluir(int id)
    {
        this.repository.Excluir(id);
        return RedirectToAction("Mostrar");
    }

}