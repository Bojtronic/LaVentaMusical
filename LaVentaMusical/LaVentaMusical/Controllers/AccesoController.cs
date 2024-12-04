using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaVentaMusical.Models;

namespace LaVentaMusical.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Acceso/Login
        [HttpPost]
        public ActionResult ProcessLogin(string email, string password)
        {
            // Usar el contexto generado por Entity Framework
            using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
            {
                // Buscar el usuario con el email y contraseña proporcionados
                var usuario = context.Usuarios.FirstOrDefault(x => x.Email == email && x.Password == password);

                if (usuario != null)
                {
                    if (usuario.Perfil == "admin")
                    {
                        // Si el usuario es administrador, redirigir a la página Home del administrador
                        return RedirectToAction("Index", "HomeAdmin");
                    }
                    else
                    {
                        // redirigir a la página Home del usuario normal
                        return RedirectToAction("Index", "HomeUsuario");
                    }
                    
                }
                else
                {
                    // Mostrar un mensaje indicando que el usuario no existe
                    ViewBag.Error = "El usuario o la contraseña son incorrectos.";
                    return View("Login");
                }
            }
        }

        // GET: Acceso/Registro
        public ActionResult Registro()
        {
            return View();
        }

        // POST: Acceso/Registro
        [HttpPost]
        public ActionResult Registro(Usuarios usuarios)
        {
            try
            {
                // Usar el contexto generado por Entity Framework
                using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
                {
                    usuarios.Perfil = "usuario";
                    context.Usuarios.Add(usuarios);
                    context.SaveChanges();
                }

                // Redirigir al usuario a la página de inicio de sesión
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                // Manejo de errores
                ViewBag.Error = $"Ocurrió un error al registrar el usuario: {ex.Message}";
                return View();
            }
        }
    }
}
