using logiWeb.Models;
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
    public JsonResult Ordem()
    {
        List<Ordem> ordens = this.repository.MostrarOrdens();
        //string ordens  = JsonSerializer.Serialize(ordens);

        return Json(ordens);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        List<Cliente> clientes = clienteRepository.Mostrar();
        string clientesJson  = JsonSerializer.Serialize(clientes);
        Console.WriteLine(clientesJson);
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