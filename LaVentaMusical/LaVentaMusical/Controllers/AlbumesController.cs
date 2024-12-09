using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaVentaMusical.Models;

namespace LaVentaMusical.Controllers
{
    public class AlbumesController : Controller
    {
        // GET: Albumes
        public ActionResult Index()
        {
            // Cargar los álbumes junto con los artistas
            var albumes = new List<Albumes>();

            // Cargar la lista de artistas para el filtro
            using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
            {
                albumes = context.Albumes.Include(a => a.Artistas).ToList();

                // Cargar la lista de artistas para el filtro
                ViewBag.Artistas = new SelectList(context.Artistas, "Id_Artista", "Nombre_Artistico");
            }

            return View(albumes);
        }


        // GET: Albumes/Create
        public ActionResult Create()
        {
            ViewBag.Id_Artista = new SelectList(new LaVentaMusicalEntities().Artistas, "Id_Artista", "Nombre_Artistico");
            return View();
        }

        // POST: Albumes/Create
        [HttpPost]
        public ActionResult Create(Albumes album, HttpPostedFileBase imagen)
        {
            try
            {
                using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
                {
                    if (imagen != null && imagen.ContentLength > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var fileExtension = Path.GetExtension(imagen.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("Imagen_Album", "Solo se permiten archivos de imagen (.jpg, .jpeg, .png, .gif).");
                            ViewBag.Id_Artista = new SelectList(context.Artistas, "Id_Artista", "Nombre_Artistico", album.Id_Artista);
                            return View(album);
                        }

                        using (var reader = new BinaryReader(imagen.InputStream))
                        {
                            album.Imagen_Album = reader.ReadBytes(imagen.ContentLength);
                        }
                    }

                    context.Albumes.Add(album);
                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ocurrió un error al agregar el álbum: {ex.Message}";
                return View();
            }
        }

        // GET: Albumes/Edit/5
        // GET: Albumes/Edit/5
        public ActionResult Edit(int id)
        {
            using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
            {
                var album = context.Albumes.Find(id);
                if (album == null) return HttpNotFound();

                // Cargar la lista de artistas y asignarla a ViewBag.Artistas
                ViewBag.Artistas = new SelectList(context.Artistas.ToList(), "Id_Artista", "Nombre_Artistico", album.Id_Artista);

                return View(album);
            }
        }

        // POST: Albumes/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Albumes album, HttpPostedFileBase imagen)
        {
            try
            {
                using (var context = new LaVentaMusicalEntities())
                {
                    var albumExistente = context.Albumes.FirstOrDefault(a => a.Id_Album == id);
                    if (albumExistente == null) return HttpNotFound();

                    albumExistente.Nombre_Album = album.Nombre_Album;
                    albumExistente.Ano_Lanzamiento = album.Ano_Lanzamiento;
                    albumExistente.Id_Artista = album.Id_Artista;

                    if (imagen != null && imagen.ContentLength > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var fileExtension = Path.GetExtension(imagen.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("Imagen_Album", "Solo se permiten archivos de imagen (.jpg, .jpeg, .png, .gif).");
                            ViewBag.Artistas = new SelectList(context.Artistas.ToList(), "Id_Artista", "Nombre_Artistico", album.Id_Artista);
                            return View(album);
                        }

                        using (var memoria = new MemoryStream())
                        {
                            imagen.InputStream.CopyTo(memoria);
                            albumExistente.Imagen_Album = memoria.ToArray();
                        }
                    }

                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Recargar los artistas en caso de error
                using (var context = new LaVentaMusicalEntities())
                {
                    ViewBag.Artistas = new SelectList(context.Artistas.ToList(), "Id_Artista", "Nombre_Artistico", album.Id_Artista);
                }

                ModelState.AddModelError("", "Ocurrió un error al actualizar el álbum: " + ex.Message);
                return View(album);
            }
        }



        // GET: Albumes/Details/5
        public ActionResult Details(int id)
        {
            using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
            {
                var album = context.Albumes.Include(a => a.Artistas).FirstOrDefault(a => a.Id_Album == id);
                if (album == null) return HttpNotFound();
                return View(album);
            }
        }

        // Obtener imagen del álbum
        public FileContentResult ObtenerImagen(int id)
        {
            using (var context = new LaVentaMusicalEntities())
            {
                var album = context.Albumes.FirstOrDefault(a => a.Id_Album == id);
                if (album != null && album.Imagen_Album != null)
                {
                    return File(album.Imagen_Album, "image/jpeg");
                }
            }

            string rutaImagenPorDefecto = Server.MapPath("~/Content/imagenes/default_album.png");
            byte[] imagenPorDefecto = System.IO.File.ReadAllBytes(rutaImagenPorDefecto);
            return File(imagenPorDefecto, "image/jpeg");
        }
    }
}
