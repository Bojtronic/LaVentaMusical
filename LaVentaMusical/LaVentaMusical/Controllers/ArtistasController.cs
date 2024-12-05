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
    public class ArtistasController : Controller
    {
        // GET: Artistas
        public ActionResult Index()
        {
            using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
            {
                var artistas = context.Artistas.ToList();
                return View(artistas);
            }
        }

        // GET: Artistas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artistas/Create
        [HttpPost]
        public ActionResult Create(Artistas artista, HttpPostedFileBase imagen)
        {
            try
            {
                using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
                {
                    // Validación de la imagen
                    if (imagen != null && imagen.ContentLength > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var fileExtension = Path.GetExtension(imagen.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("Imagen_Artista", "Solo se permiten archivos de imagen (.jpg, .jpeg, .png, .gif).");
                            return View(artista); // Devuelve la vista si hay error
                        }

                        using (var reader = new BinaryReader(imagen.InputStream))
                        {
                            artista.Imagen_Artista = reader.ReadBytes(imagen.ContentLength);
                        }
                    }

                    context.Artistas.Add(artista);
                    context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ocurrió un error al agregar el artista: {ex.Message}";
                return View();
            }
        }

        // GET: Artistas/Edit/5
        public ActionResult Edit(int id)
        {
            using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
            {
                var artista = context.Artistas.Find(id);
                if (artista == null) return HttpNotFound();
                return View(artista);
            }
        }

        // POST: Artistas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Artistas artista, HttpPostedFileBase imagen)
        {
            try
            {
                using (var context = new LaVentaMusicalEntities())
                {
                    var artistaExistente = context.Artistas.FirstOrDefault(a => a.Id_Artista == id);
                    if (artistaExistente == null)
                    {
                        return HttpNotFound();
                    }

                    // Actualiza los campos del artista
                    artistaExistente.Nombre_Artistico = artista.Nombre_Artistico;
                    artistaExistente.Fecha_Nacimiento = artista.Fecha_Nacimiento;
                    artistaExistente.Nombre_Real = artista.Nombre_Real;
                    artistaExistente.Nacionalidad = artista.Nacionalidad;

                    // Validación de la imagen
                    if (imagen != null && imagen.ContentLength > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var fileExtension = Path.GetExtension(imagen.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("Imagen_Artista", "Solo se permiten archivos de imagen (.jpg, .jpeg, .png, .gif).");
                            return View(artista); // Devuelve la vista si hay error
                        }

                        // Si se sube una nueva imagen, actualiza la base de datos
                        using (var memoria = new System.IO.MemoryStream())
                        {
                            imagen.InputStream.CopyTo(memoria);
                            artistaExistente.Imagen_Artista = memoria.ToArray();
                        }
                    }

                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al actualizar el artista: " + ex.Message);
                return View(artista);
            }
        }

        // GET: Artistas/Details/5
        public ActionResult Details(int id)
        {
            using (LaVentaMusicalEntities context = new LaVentaMusicalEntities())
            {
                var artista = context.Artistas.Find(id);
                if (artista == null) return HttpNotFound();
                return View(artista);
            }
        }

        // Obtener imagen del artista
        public FileContentResult ObtenerImagen(int id)
        {
            using (var context = new LaVentaMusicalEntities())
            {
                var artista = context.Artistas.FirstOrDefault(a => a.Id_Artista == id);
                if (artista != null && artista.Imagen_Artista != null)
                {
                    string mimeType = GetMimeType(artista.Imagen_Artista);
                    return File(artista.Imagen_Artista, mimeType);
                }
            }

            // Imagen por defecto
            string rutaImagenPorDefecto = Server.MapPath("~/Content/imagenes/default_artist.png");
            byte[] imagenPorDefecto = System.IO.File.ReadAllBytes(rutaImagenPorDefecto);
            return File(imagenPorDefecto, "image/jpeg");
        }

        // Detectar tipo MIME de la imagen
        private string GetMimeType(byte[] imageBytes)
        {
            using (var ms = new MemoryStream(imageBytes))
            {
                using (var image = System.Drawing.Image.FromStream(ms))
                {
                    if (System.Drawing.Imaging.ImageFormat.Jpeg.Equals(image.RawFormat))
                        return "image/jpeg";
                    else if (System.Drawing.Imaging.ImageFormat.Png.Equals(image.RawFormat))
                        return "image/png";
                    else if (System.Drawing.Imaging.ImageFormat.Gif.Equals(image.RawFormat))
                        return "image/gif";
                    else if (System.Drawing.Imaging.ImageFormat.Bmp.Equals(image.RawFormat))
                        return "image/bmp";
                    else
                        return "application/octet-stream";
                }
            }
        }
    }
}
