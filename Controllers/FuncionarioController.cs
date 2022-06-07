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
        List<Funcionario> funcionarios = this.repository.Mostrar(nome, id_cargo, status);
        return Json(funcionarios);
    }

    [HttpGet]
    public ActionResult MostrarPorCargo(short id_cargo)
    {
        ViewBag.Cargos = this.cargoRepository.Mostrar();
        List<Funcionario> funcionarios = this.repository.MostrarPorCargo(id_cargo);
        return View();
    }

    [HttpGet]
    public ActionResult MostrarPorCpf(string cpf)
    {
        ViewBag.Cargos = this.cargoRepository.Mostrar();
        List<Funcionario> funcionarios = this.repository.MostrarPorCpf(cpf);
        return View();
    }

    [HttpGet]
    public ActionResult MostrarPorNome(string cpf)
    {
        ViewBag.Cargos = this.cargoRepository.Mostrar();
        List<Funcionario> funcionarios = this.repository.MostrarPorCpf(cpf);
        return View();
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