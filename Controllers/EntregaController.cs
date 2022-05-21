using logiWeb.Models;
using logiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace logiWeb.Controllers; 
public class EntregaController : Controller
{
    private IEntregaRepository repository;
    private IOrdemRepository ordemRepository;

  //  private IStatusRepository statusRepository;

    public EntregaController(IEntregaRepository repository, IOrdemRepository ordemRepository, IStatusRepository statusRepository)
    {
        this.repository = repository;
        this.ordemRepository = ordemRepository;
   //     this.statusRepository = statusRepository;
    }

    [HttpGet]
    public JsonResult Index()
    {
        List<Entrega> entrega = this.repository.MostrarEntregas();
        return Json(entrega);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        ViewBag.Ordens = ordemRepository.MostrarOrdens();
        return View();
    }

    [HttpPost]
    public ActionResult Cadastrar(Entrega entrega)
    {
        this.repository.Cadastrar(entrega);
        return RedirectToAction("Mostrar");
    }

    
    public ActionResult Excluir(int id)
    {
        this.repository.Excluir(id);
        return RedirectToAction("Mostrar");
    }

    [HttpGet]
    public ActionResult Atualizar()
    {
        //ViewBag.Status = statusRepository.MostrarStatus();
        return View();
    }

    [HttpPost]
    public ActionResult Atualizar(int idEntrega, int idOrdem, int idStatus)
    {
       // this.repository.AtualizarStatus(idEntrega, idOrdem, idStatus);
        return RedirectToAction("Mostrar");
    }

}