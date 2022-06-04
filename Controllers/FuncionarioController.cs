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
    public JsonResult Todos()
    {
        List<Funcionario> funcionarios = this.repository.Mostrar();
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
    public JsonResult Cadastrar(Funcionario funcionario)
    {
        this.repository.Cadastrar(funcionario);
        
        List<Funcionario> funcionarios = this.repository.Mostrar();
        
        return Json(funcionarios);
        //return this.Todos();
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
        this.repository.Atualizar(id, funcionario);
        return RedirectToAction("Mostrar");
    }
}