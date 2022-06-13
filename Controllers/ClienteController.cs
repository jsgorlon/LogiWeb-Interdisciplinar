using logiWeb.Models;
using logiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace logiWeb.Controllers; 
public class ClienteController : Controller
{
    private IClienteRepository repository;

    public ClienteController(IClienteRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public ActionResult Index()
    {

        return View();
    } 


    [HttpGet]
    public JsonResult Todos(string? nome, int? status)
    {
        var clientes = this.repository.Mostrar(nome, status);
        return Json(clientes);
    }


    [HttpPost]
    public JsonResult Cadastrar(Cliente cliente)
    {
        var ClienteCadastrado = this.repository.Cadastrar(cliente);
        return Json(ClienteCadastrado);
    }

   

    [HttpPost]
    public JsonResult Atualizar(int id, Cliente cliente)
    {
        var cliente_atualizado = this.repository.Atualizar(id, cliente);
        return Json(cliente_atualizado);

    }

    [HttpPost]
    public JsonResult AlterarStatus(int id, int status)
    {
        var cliente_atualizado = this.repository.AlterarStatus(id, status);
        return Json(cliente_atualizado);
    }
}