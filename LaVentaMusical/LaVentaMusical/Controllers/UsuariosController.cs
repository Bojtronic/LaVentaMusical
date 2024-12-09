using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaVentaMusical.Models;

namespace LaVentaMusical.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Usuarios
        public ActionResult Index()
        {
            // Usar el contexto generado por Entity Framework
            using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
            {
                var usuarios = context.Usuarios.ToList();

                // Pasar la lista de usuarios a la vista
                return View(usuarios);
            }

        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int id)
        {
            // Usar el contexto generado por Entity Framework
            using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
            {
                var usuarios = context.Usuarios.Where(x => x.Id_Usuario == id).FirstOrDefault();

                // Pasar el usuario a la vista
                return View(usuarios);
            }
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        public ActionResult Create(Usuarios usuarios)
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
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Manejo de errores
                ViewBag.Error = $"Ocurrió un error al registrar el usuario: {ex.Message}";
                return View();
            }
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int id)
        {
            // Usar el contexto generado por Entity Framework
            using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
            {
                var usuarios = context.Usuarios.Where(x => x.Id_Usuario == id).FirstOrDefault();

                // Pasar la lista de usuarios a la vista
                return View(usuarios);
            }
           
        }

        // GET: Usuarios/Edit_1/5
        public ActionResult Edit_1(int id)
        {
            // Usar el contexto generado por Entity Framework
            using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
            {
                var usuarios = context.Usuarios.Where(x => x.Id_Usuario == id).FirstOrDefault();

                // Pasar la lista de usuarios a la vista
                return View(usuarios);
            }

        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Usuarios usuarios)
        {
            try
            {
                // Usar el contexto generado por Entity Framework
                using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
                {
                    context.Entry(usuarios).State = EntityState.Modified;
                    context.SaveChanges();
                }
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Usuarios/Edit_1/5
        [HttpPost]
        public ActionResult Edit_1(int id, Usuarios usuarios)
        {
            try
            {
                // Usar el contexto generado por Entity Framework
                using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
                {
                    context.Entry(usuarios).State = EntityState.Modified;
                    context.SaveChanges();
                }
                // TODO: Add update logic here

                return RedirectToAction("HomeUsuario");
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int id)
        {
            // Usar el contexto generado por Entity Framework
            using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
            {
                var usuarios = context.Usuarios.Where(x => x.Id_Usuario == id).FirstOrDefault();

                // Pasar la lista de usuarios a la vista
                return View(usuarios);
            }
            
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // Usar el contexto generado por Entity Framework
                using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
                {
                    Usuarios usuario = context.Usuarios.Where(x => x.Id_Usuario == id).FirstOrDefault();
                    context.Usuarios.Remove(usuario);
                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
