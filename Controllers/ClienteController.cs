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
    public JsonResult Clientes()
    {
        List<Cliente> clientes = this.repository.Mostrar();
        return Json(clientes);
    }

    [HttpGet]
    public ActionResult MostrarPorCpf(string cpf)
    {
        List<Cliente> clientes = this.repository.MostrarPorCpf(cpf);
        return View();
    }

    [HttpGet]
    public ActionResult MostrarPorNome(string nome)
    {
        List<Cliente> clientes = this.repository.MostrarPorNome(nome);
        return View();
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Cadastrar(Cliente cliente)
    {
        this.repository.Cadastrar(cliente);
        return RedirectToAction("Index");
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