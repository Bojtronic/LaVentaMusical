using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using LaVentaMusical.Models;

namespace LaVentaMusical.Controllers
{
    public class VentaCancionesController : Controller
    {
        private static List<Canciones> carrito = new List<Canciones>();

        // GET: VentaCanciones
        public ActionResult Index()
        {
            using (var context = new LaVentaMusicalEntities())
            {
                // Carga las relaciones con Albumes y Artistas
                var canciones = context.Canciones
                                       .Where(c => c.Canciones_Disponibles > 0)
                                       .Include(c => c.Albumes) // Relación con Albumes
                                       .Include(c => c.Albumes.Artistas) // Relación con Artistas a través de Albumes
                                       .ToList();

                return View(canciones);
            }
        }

        public ActionResult Index_1()
        {
            using (var context = new LaVentaMusicalEntities())
            {
                // Carga las relaciones con Albumes y Artistas
                var canciones = context.Canciones
                                       .Where(c => c.Canciones_Disponibles > 0)
                                       .Include(c => c.Albumes) // Relación con Albumes
                                       .Include(c => c.Albumes.Artistas) // Relación con Artistas a través de Albumes
                                       .ToList();

                return View(canciones);
            }
        }

        // GET: Agregar canción al carrito
        [HttpGet]
        public ActionResult AgregarAlCarrito(int id)
        {
            using (var context = new LaVentaMusicalEntities())
            {
                var cancion = context.Canciones
                    .Include(c => c.Albumes)
                    .Include(c => c.Albumes.Artistas)
                    .FirstOrDefault(c => c.Id_Cancion == id);

                if (cancion != null && cancion.Canciones_Disponibles > 0)
                {
                    carrito.Add(cancion);
                }
                else
                {
                    TempData["Error"] = "La canción no está disponible o no existe.";
                }
            }
            return RedirectToAction("Carrito");
        }

        [HttpGet]
        public ActionResult AgregarAlCarrito_1(int id)
        {
            using (var context = new LaVentaMusicalEntities())
            {
                var cancion = context.Canciones
                    .Include(c => c.Albumes)
                    .Include(c => c.Albumes.Artistas)
                    .FirstOrDefault(c => c.Id_Cancion == id);

                if (cancion != null && cancion.Canciones_Disponibles > 0)
                {
                    carrito.Add(cancion);
                }
                else
                {
                    TempData["Error"] = "La canción no está disponible o no existe.";
                }
            }
            return RedirectToAction("Carrito_1");
        }



        // GET: Carrito
        public ActionResult Carrito()
        {
            using (var context = new LaVentaMusicalEntities())
            {
                var canciones = carrito.Select(c => new CancionViewModel
                {
                    NombreCancion = c.Nombre_Cancion,
                    NombreArtista = c.Albumes.Artistas.Nombre_Artistico,
                    NombreAlbum = c.Albumes.Nombre_Album,
                    Precio = c.Precio
                }).ToList();

                ViewBag.Subtotal = canciones.Sum(c => c.Precio ?? 0);
                ViewBag.IVA = ViewBag.Subtotal * 0.13m;
                ViewBag.Total = ViewBag.Subtotal + ViewBag.IVA;

                return View(canciones);
            }
        }

        public ActionResult Carrito_1()
        {
            using (var context = new LaVentaMusicalEntities())
            {
                var canciones = carrito.Select(c => new CancionViewModel
                {
                    NombreCancion = c.Nombre_Cancion,
                    NombreArtista = c.Albumes.Artistas.Nombre_Artistico,
                    NombreAlbum = c.Albumes.Nombre_Album,
                    Precio = c.Precio
                }).ToList();

                ViewBag.Subtotal = canciones.Sum(c => c.Precio ?? 0);
                ViewBag.IVA = ViewBag.Subtotal * 0.13m;
                ViewBag.Total = ViewBag.Subtotal + ViewBag.IVA;

                return View(canciones);
            }
        }

        // POST: Eliminar canción del carrito
        [HttpPost]
        public ActionResult EliminarDelCarrito(int id)
        {
            var cancion = carrito.FirstOrDefault(c => c.Id_Cancion == id);
            if (cancion != null)
            {
                carrito.Remove(cancion);
            }
            return RedirectToAction("Carrito");
        }

        [HttpPost]
        public ActionResult EliminarDelCarrito_1(int id)
        {
            var cancion = carrito.FirstOrDefault(c => c.Id_Cancion == id);
            if (cancion != null)
            {
                carrito.Remove(cancion);
            }
            return RedirectToAction("Carrito_1");
        }



        // POST: Facturar
        [HttpPost]
        public ActionResult Facturar(string tipoPago, string tarjeta, string codigoTarjeta)
        {
            if (carrito.Count == 0 || string.IsNullOrEmpty(tipoPago))
            {
                ModelState.AddModelError("", "Debe seleccionar canciones y un método de pago.");
                return RedirectToAction("Carrito");
            }

            if (tipoPago == "Tarjeta" && (string.IsNullOrEmpty(tarjeta) || string.IsNullOrEmpty(codigoTarjeta)))
            {
                ModelState.AddModelError("", "Debe proporcionar los datos de la tarjeta.");
                return RedirectToAction("Carrito");
            }

            // Calcular totales
            var subtotal = carrito.Sum(c => c.Precio);
            var iva = subtotal * 0.13m;
            var total = subtotal + iva;

            if (tipoPago == "Tarjeta")
            {
                total += subtotal * 0.02m; // 2% extra por pago con tarjeta
                tarjeta = EnmascararTarjeta(tarjeta); // Enmascarar tarjeta
            }

            // Generar la factura
            using (var context = new LaVentaMusicalEntities())
            {
                var usuario = ObtenerUsuario(context);
                if (usuario == null)
                {
                    ModelState.AddModelError("", "Usuario no encontrado.");
                    return RedirectToAction("Carrito");
                }

                var factura = new Facturas
                {
                    Id_Usuario = usuario.Id_Usuario,
                    FechaCompra = DateTime.Now,
                    NumeroFactura = Guid.NewGuid().ToString().Substring(0, 10).ToUpper(),
                    SubTotal = subtotal,
                    IVA = iva,
                    Total = (decimal)total,
                    Canciones = carrito.ToList()
                };

                // Validación y creación de pagos
                Pagos pago = new Pagos
                {
                    MetodoPago = tipoPago, // Asignar tipo de pago (Tarjeta o Otro)
                    MontoPagado = total,   // Asignar monto pagado
                    FechaPago = DateTime.Now
                };

                // Validación para "Tarjeta"
                if (tipoPago == "Tarjeta")
                {
                    if (string.IsNullOrEmpty(codigoTarjeta))
                    {
                        ModelState.AddModelError("", "Debe proporcionar el código de seguridad de la tarjeta.");
                        return RedirectToAction("Carrito");
                    }
                    pago.CodigoAutorizacion = codigoTarjeta; // Asignar el código de seguridad de la tarjeta
                }

                // Agregar el pago a la factura
                factura.Pagos = new List<Pagos> { pago };

                // Agregar la factura al contexto y guardar cambios
                context.Facturas.Add(factura);

                // Reducir la cantidad disponible de las canciones
                foreach (var cancion in carrito)
                {
                    var dbCancion = context.Canciones.Find(cancion.Id_Cancion);
                    if (dbCancion.Canciones_Disponibles <= 0)
                    {
                        ModelState.AddModelError("", $"La canción {cancion.Nombre_Cancion} no está disponible.");
                        return RedirectToAction("Carrito");
                    }
                    dbCancion.Canciones_Disponibles--; // Reducir cantidad disponible
                }

                context.SaveChanges();

                // Vaciar el carrito y redirigir
                carrito.Clear();
                TempData["Factura"] = factura;
            }

            return RedirectToAction("Factura");
        }

        // GET: Factura generada
        public ActionResult Factura()
        {
            var factura = TempData["Factura"] as Facturas;
            if (factura == null)
            {
                return RedirectToAction("Index");
            }
            return View(factura);
        }

        // POST: Cancelar compra
        [HttpPost]
        public ActionResult CancelarCompra()
        {
            carrito.Clear();
            return RedirectToAction("Index");
        }

        // Método para enmascarar tarjeta
        private string EnmascararTarjeta(string tarjeta)
        {
            if (tarjeta.Length < 4) return "****";
            return new string('*', tarjeta.Length - 4) + tarjeta.Substring(tarjeta.Length - 4);
        }

        // Método para obtener el usuario logueado
        private Usuarios ObtenerUsuario(LaVentaMusicalEntities context)
        {
            // Lógica para obtener el usuario actual (simulación)
            return context.Usuarios.FirstOrDefault(u => u.Nombre_Usuario == User.Identity.Name);
        }

        public class CancionViewModel
        {
            public string NombreCancion { get; set; }
            public string NombreArtista { get; set; }
            public string NombreAlbum { get; set; }
            public decimal? Precio { get; set; }
        }

    }
}
