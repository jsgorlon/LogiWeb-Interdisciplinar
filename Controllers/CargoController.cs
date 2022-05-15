using logiWeb.Models;
using logiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace logiWeb.Controllers
{
    public class CargoController: Controller
    {
        private ICargoRepository repository;

        public CargoController(ICargoRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public JsonResult Index()
        {
            List<Cargo> cargos = this.repository.Mostrar();
            return Json(cargos);
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(Cargo cargo)
        {
            this.repository.Cadastrar(cargo);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Atualizar(int id)
        {
            Cargo cargo = this.repository.Mostrar(id);
            return Json(cargo);
        }

        public ActionResult Atualizar(int id, Cargo cargo)
        {
            this.repository.Atualizar(id, cargo);
            return RedirectToAction("Index");
        }

        public ActionResult Excluir(int id)
        {
            this.repository.Excluir(id);
            return RedirectToAction("Index");
        }
    }
}