using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaVentaMusical.Models;

namespace LaVentaMusical.Controllers
{
    public class CancionesController : Controller
    {
        // GET: Canciones
        public ActionResult Index()
        {
            using (var context = new LaVentaMusicalEntities())
            {
                var canciones = context.Canciones
                                       .Include(c => c.Albumes)
                                       .Include(c => c.Albumes.Artistas)
                                       .Include(c => c.Generos)
                                       .ToList();
                return View(canciones);
            }
        }

        // GET: Canciones/Create
        public ActionResult Create()
        {
            PrepararSelectLists();
            return View();
        }

        // POST: Canciones/Create
        [HttpPost]
        public ActionResult Create(Canciones cancion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var context = new LaVentaMusicalEntities())
                    {
                        context.Canciones.Add(cancion);
                        context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al agregar la canción: " + ex.Message);
                }
            }

            PrepararSelectLists(cancion.Id_Genero, cancion.Id_Album);
            return View(cancion);
        }

        // GET: Canciones/Edit/5
        public ActionResult Edit(int id)
        {
            using (var context = new LaVentaMusicalEntities())
            {
                var cancion = context.Canciones.Find(id);
                if (cancion == null)
                {
                    return HttpNotFound();
                }

                PrepararSelectLists(cancion.Id_Genero, cancion.Id_Album);
                return View(cancion);
            }
        }

        // POST: Canciones/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Canciones cancionActualizada)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var context = new LaVentaMusicalEntities())
                    {
                        var cancion = context.Canciones.Find(id);
                        if (cancion == null)
                        {
                            return HttpNotFound();
                        }

                        cancion.Id_Genero = cancionActualizada.Id_Genero;
                        cancion.Id_Album = cancionActualizada.Id_Album;
                        cancion.Nombre_Cancion = cancionActualizada.Nombre_Cancion;
                        cancion.Video_URL = cancionActualizada.Video_URL;
                        cancion.Precio = cancionActualizada.Precio;
                        cancion.Canciones_Disponibles = cancionActualizada.Canciones_Disponibles;

                        context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar la canción: " + ex.Message);
                }
            }

            PrepararSelectLists(cancionActualizada.Id_Genero, cancionActualizada.Id_Album);
            return View(cancionActualizada);
        }

        // GET: Canciones/Details/5
        public ActionResult Details(int id)
        {
            using (var context = new LaVentaMusicalEntities())
            {
                var cancion = context.Canciones
                                     .Include(c => c.Albumes)
                                     .Include(c => c.Albumes.Artistas)
                                     .Include(c => c.Generos)
                                     .FirstOrDefault(c => c.Id_Cancion == id);

                if (cancion == null)
                {
                    return HttpNotFound();
                }

                return View(cancion);
            }
        }

        // GET: Canciones/Delete/5
        public ActionResult Delete(int id)
        {
            using (var context = new LaVentaMusicalEntities())
            {
                var cancion = context.Canciones
                                     .Include(c => c.Albumes)
                                     .Include(c => c.Albumes.Artistas)
                                     .Include(c => c.Generos)
                                     .FirstOrDefault(c => c.Id_Cancion == id);

                if (cancion == null)
                {
                    return HttpNotFound();
                }

                return View(cancion);
            }
        }

        // POST: Canciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (var context = new LaVentaMusicalEntities())
                {
                    var cancion = context.Canciones.Find(id);
                    if (cancion == null)
                    {
                        return HttpNotFound();
                    }

                    context.Canciones.Remove(cancion);
                    context.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar la canción: " + ex.Message);
                return View();
            }
        }


        private void PrepararSelectLists(int? idGeneroSeleccionado = null, int? idAlbumSeleccionado = null)
        {
            using (var context = new LaVentaMusicalEntities())
            {
                ViewBag.Id_Genero = new SelectList(context.Generos.ToList(), "Id_Genero", "Descripcion", idGeneroSeleccionado);
                ViewBag.Id_Album = new SelectList(context.Albumes.ToList(), "Id_Album", "Nombre_Album", idAlbumSeleccionado);
            }
        }
    }
}
