using logiWeb.Models;
using Microsoft.AspNetCore.Mvc;
using logiWeb.Repositories;

namespace logiWeb.Controllers
{
    public class cargoController : Controller
    {
        public ActionResult Index()
        {
            CargoRepository repository = new CargoRepository();
            List<Cargo> cargos = repository.Read();
            return View(cargos);

        }
    }
}