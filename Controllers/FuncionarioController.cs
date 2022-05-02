using logiWeb.Models;
using logiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace logiWeb.Controllers; 
public class FuncionarioController : Controller
{
    private FuncionarioSqlRepository repository = new FuncionarioSqlRepository();

    [HttpGet]
    public JsonResult Mostrar()
    {
        List<Funcionario> funcionarios = this.repository.Mostrar();
        return Json(funcionarios);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Cadastrar(IFormCollection form) //TODO Buscar forma melhor de instanciar as classes
    {//TODO Avalicar outra forma de instanciar classe
        if (form.ContainsKey("Atendente"))
        {
            Atendente atendente = new Atendente();
            atendente.Nome = form["Nome"];
            atendente.Email = form["Email"];
            atendente.Cpf = form["Cpf"];
            atendente.Rg = form["Rg"];
            atendente.DatNasc = DateOnly.Parse(form["DatNasc"]);
            atendente.BonusExtra = double.Parse(form["BonusExtra"]);
            atendente.Login = form["Login"];
            atendente.Senha = form["Senha"];
            atendente.DatCad = DateTime.Now;
            atendente.Ativo = true;
            this.repository.Cadastrar(atendente);
        }
        else if (form.ContainsKey("Motorista"))
        {
            Motorista motorista = new Motorista();
            motorista.Nome = form["Nome"];
            motorista.Email = form["Email"];
            motorista.Cpf = form["Cpf"];
            motorista.Rg = form["Rg"];
            motorista.DatNasc = DateOnly.Parse(form["DatNasc"]);
            motorista.BonusExtra = double.Parse(form["BonusExtra"]);
            motorista.Login = form["Login"];
            motorista.Senha = form["Senha"];
            motorista.DatCad = DateTime.Now;
            motorista.Ativo = true;
            this.repository.Cadastrar(motorista);
        }
        else
        {
            OperadorLogistico operador = new OperadorLogistico();
            operador.Nome = form["Nome"];
            operador.Email = form["Email"];
            operador.Cpf = form["Cpf"];
            operador.Rg = form["Rg"];
            operador.DatNasc = DateOnly.Parse(form["DatNasc"]);
            operador.BonusExtra = double.Parse(form["BonusExtra"]);
            operador.Login = form["Login"];
            operador.Senha = form["Senha"];
            operador.DatCad = DateTime.Now;
            operador.Ativo = true;
            this.repository.Cadastrar(operador);
        }
        return RedirectToAction("Mostrar");
    }

    
    public ActionResult Excluir(int id)
    {
        this.repository.Excluir(id);
        return RedirectToAction("Mostrar");
    }

    [HttpGet]
    public JsonResult Atualizar(int id)
    {
        var funcionario = this.repository.Mostrar();
        return Json(funcionario);
    }

    [HttpPost]
    public ActionResult Atualizar(int id, Funcionario funcionario)
    {
        this.repository.Atualizar(id, funcionario); //TODO 
        return RedirectToAction("Mostrar");
    }
}