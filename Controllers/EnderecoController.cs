using logiWeb.Models;
using logiWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace logiWeb.Controllers
{
    public class EnderecoController: Controller
    {
        private IEnderecoRepository repository;

        public EnderecoController(IEnderecoRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public JsonResult MostrarEstado()
        {
            List<Endereco> endereco = this.repository.MostrarEstado();
            return Json(endereco);
        }

        [HttpGet]
        public JsonResult MostrarCidade(int id_estado)
        {
            List<Endereco> endereco = this.repository.MostrarCidade(id_estado);
            return Json(endereco);
        }
    }
}