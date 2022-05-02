using logiWeb.Models;
using logiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace logiWeb.Controllers; 
public class ClienteController : Controller
{
    private ClienteRepository repository = new ClienteRepository();

    [HttpGet]
    public JsonResult Index()
    {
        // #TODO retorna a visualização de ClientesFisicos apenas como um placeholder
        List<ClienteFisico> clientes = this.repository.MostrarClientesFisicos();
        return Json(clientes);
    }

    [HttpGet]
    public JsonResult IndexClientesFisico()
    {
        List<ClienteFisico> clientes = this.repository.MostrarClientesFisicos();
        return Json(clientes);
    }

    public JsonResult IndexClientesJuridico()
    {
        List<ClienteJuridico> clientes = this.repository.MostrarClientesJuridicos();
        return Json(clientes);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Cadastrar(IFormCollection form)
    {
        if (form.ContainsKey("Cpf"))
        {
            ClienteFisico cliente = new ClienteFisico();
            cliente.Nome = form["Nome"];
            cliente.Email = form["Email"];
            cliente.Cpf = form["Cpf"];
            cliente.Rg = form["Rg"];
            cliente.DatNasc = DateOnly.Parse(form["DatNasc"]);
            this.repository.CadastrarClienteFisico(cliente);
        }
        else
        {
            ClienteJuridico cliente = new ClienteJuridico();
            cliente.Nome = form["Nome"];
            cliente.Email = form["Email"];
            cliente.Cnpj = form["Cnpj"];
            cliente.RazaoSocial = form["RazaoSocial"];
            this.repository.CadastrarClienteJuridico(cliente);
        }
        return RedirectToAction("Index");
    }
}