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
    public JsonResult Ordem(string? nome)
    {
        List<Ordem> ordens = this.repository.MostrarOrdens(nome);
        return Json(ordens);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        List<Cliente> clientes = clienteRepository.Mostrar();

        return View();
    }

    [HttpPost]
    public string Cadastrar(Ordem ordem)
    {
        // ordem.IdFuncionario = session(id) funcionario logado
        //verificar se endereco ja existe, caso nao adicionar o endereco
        return this.repository.Cadastrar(ordem);
    }

    
    public ActionResult Excluir(int id)
    {
        this.repository.Excluir(id);
        return RedirectToAction("Mostrar");
    }

}