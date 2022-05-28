using logiWeb.Models;
using logiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace logiWeb.Controllers; 
public class EntregaController : Controller
{
    private IEntregaRepository repository;
    private IOrdemRepository ordemRepository;
    private IStatusRepository statusRepository;

    public EntregaController(IEntregaRepository repository, IOrdemRepository ordemRepository, IStatusRepository statusRepository)
    {
        this.repository = repository;
        this.ordemRepository = ordemRepository;
        this.statusRepository = statusRepository;
    }

    [HttpGet]
    public ActionResult Index()
    {
        List<Entrega> entregas = this.repository.MostrarEntregas();
        string entregasJson  = JsonSerializer.Serialize(entregas);
        Console.WriteLine(entregasJson);
        return View();
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        ViewBag.Ordens = ordemRepository.MostrarOrdens();
        return View();
    }

    [HttpPost]
    public ActionResult Cadastrar(Entrega entrega, int[] idOrdem)
    {
        this.repository.Cadastrar(entrega, idOrdem);
        return RedirectToAction("Mostrar");
    }

    
    public ActionResult Excluir(int id)
    {
        this.repository.Excluir(id);
        return RedirectToAction("Mostrar");
    }

    [HttpGet]
    public ActionResult AtualizarOrdens()
    {
        List<Status> status = statusRepository.Mostrar();
        string statusJson  = JsonSerializer.Serialize(status);
        return View();
    }

    [HttpPost]
    public ActionResult AtualizarOrdens(Ordem ordem)
    {
        this.repository.StatusOrdem(ordem);
        return RedirectToAction("Mostrar");
    }
    public ActionResult AtualizarEntregas()
    {
        List<Status> status = statusRepository.Mostrar();
        string statusJson  = JsonSerializer.Serialize(status);
        return View();
    }

    [HttpPost]
    public ActionResult AtualizarEntregas(Entrega entrega)
    {
        this.repository.StatusEntrega(entrega);
        return RedirectToAction("Mostrar");
    }

}