using logiWeb.Models;
using logiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
    public JsonResult Clientes()
    {
        List<Cliente> clientes = this.repository.Mostrar();
        return Json(clientes);
    }


    [HttpPost]
    public JsonResult Cadastrar(Cliente cliente)
    {
        this.repository.Cadastrar(cliente);
        return Json(new {
            error = false, 
        });
    }

    [HttpGet]
    public JsonResult Atualizar(int id)
    {
        var cliente = this.repository.Mostrar(id);
        return Json(cliente);
    }

    [HttpPost]
    public ActionResult Atualizar(int id, Cliente cliente)
    {
        this.repository.Atualizar(id, cliente);
        return RedirectToAction("Index");

    }

    [HttpDelete]
    public ActionResult Excluir(int id)
    {
        this.repository.Excluir(id);
        return RedirectToAction("Index");
    }
}