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

    public ActionResult Index()
    {
        ViewBag.IdFuncionario =  HttpContext.Session.GetString("id_funcionario"); 
        return View();
    }


    public JsonResult Entregas(int? id_funcionario, int? id_motorista)
    {
        var entregas = this.repository.MostrarEntregas(id_funcionario, id_motorista); 
        return Json(entregas);
    }

 

    [HttpPost]
    public JsonResult Cadastrar(int id_funcionario, int id_motorista)
    {
       return Json( this.repository.Cadastrar(id_funcionario, id_motorista)); 
    }

    [HttpPost]
    public JsonResult AdicionarOrdem(int id_entrega, int id_ordem){

        return Json(this.repository.AdicionarOrdem(id_entrega, id_ordem));
    }

    [HttpPost]
    public JsonResult ObterEntregaOrdem(int id_entrega){

        return Json(this.repository.MostrarEntregaOrdem(id_entrega));
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
    [HttpGet]
    public ActionResult Detalhe(int id)
    {
        Entrega entregas = this.repository.MostrarDetalheEntrega(id);
        string entregasJson  = JsonSerializer.Serialize(entregas);
        Console.WriteLine(entregasJson);
        return View();
    }

}