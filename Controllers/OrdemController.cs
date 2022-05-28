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

    [HttpGet]
    public ActionResult Index()
    {
        List<Ordem> ordens = this.repository.MostrarOrdens();
        string ordensJson  = JsonSerializer.Serialize(ordens);
        Console.WriteLine(ordensJson);
        return View();
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