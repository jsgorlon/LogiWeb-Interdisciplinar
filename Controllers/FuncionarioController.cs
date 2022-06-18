using logiWeb.Models;
using logiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace logiWeb.Controllers; 
public class FuncionarioController : Controller
{
    private IFuncionarioRepository repository;
    private ICargoRepository cargoRepository;

    public FuncionarioController(IFuncionarioRepository repository, ICargoRepository cargoRepository)
    {
        this.repository = repository;
        this.cargoRepository = cargoRepository;
    }
     public ActionResult Index()
     {
        return View(); 
     }

    [HttpGet]
    public JsonResult Todos(string? nome, int? id_cargo, int? status)
    {
        var funcionarios = this.repository.Mostrar(nome, id_cargo, status);
        return Json(funcionarios);
    }

  

    [HttpPost]
    public string Cadastrar(Funcionario funcionario)
    {   
      return this.repository.Cadastrar(funcionario);
    }

    
    public void AlterarStatus(int id, int status)
    {
      this.repository.AlterarStatus(id, status);
    }

    [HttpPost]
    public string Atualizar(int id, Funcionario funcionario)
    {
     return this.repository.Atualizar(id, funcionario);
    }
}