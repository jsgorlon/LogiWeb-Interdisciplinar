using logiWeb.Models;
using logiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace logiWeb.Controllers; 
public class FuncionarioController : Controller
{
    private IFuncionarioRepository repository;

    public FuncionarioController(IFuncionarioRepository repository)
    {
        this.repository = repository;
    }

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
    public ActionResult Cadastrar(Funcionario funcionario)
    {
        this.repository.Cadastrar(funcionario);
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