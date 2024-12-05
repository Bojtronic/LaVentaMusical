using LaVentaMusical.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace LaVentaMusical.Controllers
{
    public class GenerosController : Controller
    {
        // GET: Generos/Index
        public ActionResult Index()
        {
            using (var context = new LaVentaMusicalEntities())
            {
                var generos = context.Generos.ToList();
                return View(generos);
            }
        }

        // GET: Generos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Generos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Generos genero)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(genero);
                }

                using (var context = new LaVentaMusicalEntities())
                {
                    context.Generos.Add(genero);
                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ocurrió un error al registrar el género: {ex.Message}";
                return View();
            }
        }

        // GET: Generos/Edit/5
        public ActionResult Edit(int id)
        {
            using (var context = new LaVentaMusicalEntities())
            {
                var genero = context.Generos.FirstOrDefault(g => g.Id_Genero == id);
                if (genero == null)
                {
                    return HttpNotFound();
                }
                return View(genero);
            }
        }

        // POST: Generos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Generos genero)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(genero);
                }

                using (var context = new LaVentaMusicalEntities())
                {
                    var generoExistente = context.Generos.FirstOrDefault(g => g.Id_Genero == genero.Id_Genero);
                    if (generoExistente != null)
                    {
                        generoExistente.Descripcion = genero.Descripcion;
                        context.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ocurrió un error al actualizar el género: {ex.Message}";
                return View();
            }
        }

        // GET: Generos/Details/5
        public ActionResult Details(int id)
        {
            using (var context = new LaVentaMusicalEntities())
            {
                var genero = context.Generos.FirstOrDefault(g => g.Id_Genero == id);
                if (genero == null)
                {
                    return HttpNotFound();
                }
                return View(genero);
            }
        }

        // GET: Generos/Delete/5
        public ActionResult Delete(int id)
        {
            using (var context = new LaVentaMusicalEntities())
            {
                var genero = context.Generos.FirstOrDefault(g => g.Id_Genero == id);
                if (genero == null)
                {
                    return HttpNotFound();
                }
                return View(genero);
            }
        }

        // POST: Generos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (var context = new LaVentaMusicalEntities())
                {
                    var genero = context.Generos.FirstOrDefault(g => g.Id_Genero == id);
                    if (genero != null)
                    {
                        context.Generos.Remove(genero);
                        context.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ocurrió un error al eliminar el género: {ex.Message}";
                return View();
            }
        }
    }
}
