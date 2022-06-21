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

        return View();
    }


    public JsonResult Entregas(int? id_funcionario, int? id_motorista)
    {
        var entregas = this.repository.MostrarEntregas(id_funcionario, id_motorista); 
        return Json(entregas);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
       // ViewBag.Ordens = ordemRepository.MostrarOrdens();
        return View();
    }

    [HttpPost]
    public ActionResult Cadastrar(Entrega entrega, int[] idOrdem)
    {
        this.repository.Cadastrar(entrega, idOrdem);
        return RedirectToAction("Mostrar");
    }

    
    public JsonResult Excluir(int id)
    {
        return Json(this.repository.Excluir(id));
    
    }

    public JsonResult AtualizarOrdens(int id_ordem, int id_status, int id_entrega)
    {  
        var entregas = this.repository.StatusOrdem(id_ordem, id_status, id_entrega); 
        List<Ordem> ordensEntrega = new List<Ordem>();
        ordensEntrega = this.repository.MostrarOrdensEntrega(id_entrega);
        int contPendentes = 0;
        int contAguardo = 0;
        int contCaminho = 0;
        int contEntregues = 0;
        int contAusente = 0;
        int statusEntrega = 0;
        foreach (var item in ordensEntrega)
        {
            if (item.IdStatus == 1)
            {
                contPendentes++;
            }else if (item.IdStatus == 2)
            {
                contAguardo++;
            }else if (item.IdStatus == 3)
            {
                contCaminho++;
            }else if (item.IdStatus == 4)
            {
                contEntregues++;
            }else if (item.IdStatus == 5)
            {
                contAusente++;
            }
        }
        if (contEntregues == ordensEntrega.Count)
        {
            statusEntrega = 8;
        }else if (contAguardo > 0 )
        {
            statusEntrega = 10;
        }else if (contEntregues > 0 &&  contAusente > 0 && contPendentes == 0 && contAguardo == 0 && contCaminho == 0)
        {
            statusEntrega = 9;
        }else if (contCaminho > 0 && contAguardo == 0)
        {
            statusEntrega = 7;
        }else if (contEntregues > 0 && contPendentes > 0 && contAguardo == 0)
        {
            statusEntrega = 7;
        }
        
        if (statusEntrega != 0)
        {
            this.repository.StatusEntrega(id_entrega, statusEntrega);
        }
        return Json(entregas);
        
    }
    public ActionResult AtualizarEntregas()
    {
        var status = statusRepository.Mostrar();
        string statusJson  = JsonSerializer.Serialize(status);
        return View();
    }

    public ActionResult Detalhe(int id)
    {
        var ordens = this.repository.MostrarDetalheEntrega(id); 
        return Json(ordens);
    }

    public JsonResult Status()
    {
        var status = this.statusRepository.Mostrar(); 
        return Json(status);
    }

}