using logiWeb.Models;
using logiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace logiWeb.Controllers; 
public class ClienteController : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        // #TODO retorna a visualização de ClientesFisicos apenas como um placeholder
        ClienteRepository repository = new ClienteRepository();
        List<ClienteFisico> clientes = repository.MostrarClientesFisicos();
        return View(clientes);
    }

    [HttpGet]
    public ActionResult IndexClienteFisico()
    {
        ClienteRepository repository = new ClienteRepository();
        List<ClienteFisico> clientes = repository.MostrarClientesFisicos();
        return View(clientes);
    }

    public ActionResult IndexClienteJuridico()
    {
        ClienteRepository repository = new ClienteRepository();
        List<ClienteJuridico> clientes = repository.MostrarClientesJuridicos();
        return View(clientes);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Cadastrar(IFormCollection form)
    {
        ClienteRepository repository = new ClienteRepository();
        if (form.ContainsKey("Cpf"))
        {
            ClienteFisico cliente = new ClienteFisico();
            cliente.Nome = form["Nome"];
            cliente.Email = form["Email"];
            cliente.Cpf = form["Cpf"];
            cliente.Rg = form["Rg"];
            cliente.DatNasc = DateOnly.Parse(form["DatNasc"]);
            repository.CadastrarClienteFisico(cliente);
        }
        else
        {
            ClienteJuridico cliente = new ClienteJuridico();
            cliente.Nome = form["Nome"];
            cliente.Email = form["Email"];
            cliente.Cnpj = form["Cnpj"];
            cliente.RazaoSocial = form["RazaoSocial"];
            repository.CadastrarClienteJuridico(cliente);
        }
        return RedirectToAction("Index");
    }
}