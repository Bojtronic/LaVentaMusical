using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaVentaMusical.Controllers
{
    public class HomeUsuarioController : Controller
    {
        // GET: HomeUsuario
        public ActionResult Index()
        {
            var usuarioId = TempData["Id_Usuario"];
            return View(usuarioId);
        }
    }
}
