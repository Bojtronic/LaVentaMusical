using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LaVentaMusical.Models;

namespace LaVentaMusical.Controllers
{
    public class FacturasController : Controller
    {
        // Método común para generar un PDF
        private byte[] GenerarFacturaPDF(Facturas factura)
        {
            using (var stream = new MemoryStream())
            {
                var document = new Document();
                PdfWriter.GetInstance(document, stream);
                document.Open();

                document.Add(new Paragraph($"Factura: {factura.NumeroFactura}"));
                document.Add(new Paragraph($"Fecha: {factura.FechaCompra:dd/MM/yyyy}"));
                document.Add(new Paragraph($"Cliente: {factura.Usuarios.Nombre_Usuario}"));
                document.Add(new Paragraph($"Subtotal: {factura.SubTotal:C}"));
                document.Add(new Paragraph($"IVA: {factura.IVA:C}"));
                document.Add(new Paragraph($"Total: {factura.Total:C}"));

                document.Add(new Paragraph("Detalles:"));
                var table = new PdfPTable(4) { WidthPercentage = 100 };
                table.AddCell("Canción");
                table.AddCell("Precio Unitario");
                table.AddCell("Cantidad");
                table.AddCell("Subtotal");

                foreach (var detalle in factura.DetalleFactura)
                {
                    table.AddCell(detalle.Canciones.Nombre_Cancion);
                    table.AddCell(detalle.PrecioUnitario.ToString());
                    table.AddCell(detalle.Cantidad.ToString());
                    table.AddCell(detalle.Subtotal.ToString());
                }

                document.Add(table);
                document.Close();

                return stream.ToArray();
            }
        }

        // GET: Facturas
        public ActionResult Index()
        {
            try
            {
                using (var context = new LaVentaMusicalEntities())
                {
                    // Obtén las facturas desde la base de datos y mapea a HistorialFacturaViewModel
                    var facturas = context.Facturas
                        .Select(f => new HistorialFacturaViewModel
                        {
                            NumeroFactura = f.NumeroFactura,
                            FechaCompra = f.FechaCompra ?? DateTime.MinValue, // Asegúrate de manejar posibles valores nulos
                            Usuario = f.Usuarios.Nombre_Usuario,
                            SubTotal = f.SubTotal ?? 0,  // Si el subtotal es nulo, asigna 0
                            IVA = f.IVA ?? 0,            // Si el IVA es nulo, asigna 0
                            Total = f.Total
                        })
                        .ToList();

                    // Pasa la lista de facturas como modelo a la vista
                    return View(facturas);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                TempData["MensajeError"] = "Ocurrió un error al cargar las facturas: " + ex.Message;
                return RedirectToAction("Index"); // Redirige al mismo índice en caso de error
            }
        }



        // Acción para mostrar el historial de compras
        public ActionResult HistorialCompras(int? userId)
        {
            try
            {
                using (var context = new LaVentaMusicalEntities())
                {
                    var facturasQuery = context.Facturas
                        .Where(f => !userId.HasValue || f.Id_Usuario == userId.Value)
                        .Select(f => new HistorialFacturaViewModel
                        {
                            NumeroFactura = f.NumeroFactura,
                            FechaCompra = f.FechaCompra ?? DateTime.MinValue,
                            Usuario = f.Usuarios.Nombre_Usuario,
                            SubTotal = f.SubTotal ?? 0,
                            IVA = f.IVA ?? 0,
                            Total = f.Total
                        })
                        .ToList(); // Ejecutar la consulta y obtener los resultados

                    // Asegúrate de que los usuarios están siendo correctamente asignados a ViewBag.Usuarios
                    ViewBag.Usuarios = context.Usuarios.ToList();

                    if (facturasQuery == null || !facturasQuery.Any())
                    {
                        TempData["MensajeError"] = "No se encontraron facturas.";
                        return View(); // Mostrar la vista sin datos si no se encuentran facturas
                    }

                    return View(facturasQuery);
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Ocurrió un error al cargar el historial de compras: " + ex.Message;
                return RedirectToAction("Index");
            }
        }




        // Acción para generar un PDF de la factura
        public ActionResult GenerarPDF(string numeroFactura)
        {
            try
            {
                using (var context = new LaVentaMusicalEntities())
                {
                    // Obtener la factura y las relaciones de Usuario, DetalleFactura y Canciones
                    var factura = context.Facturas
                        .Where(f => f.NumeroFactura == numeroFactura)
                        .Select(f => new
                        {
                            Factura = f,
                            Usuario = f.Usuarios,
                            DetallesFactura = f.DetalleFactura.Select(d => new
                            {
                                Detalle = d,
                                Canciones = d.Canciones
                            }).ToList()
                        })
                        .FirstOrDefault();

                    // Verificar si la factura existe
                    if (factura == null)
                    {
                        return HttpNotFound("Factura no encontrada.");
                    }

                    // Generar el PDF con los datos de la factura
                    var pdfBytes = GenerarFacturaPDF(factura.Factura);

                    // Devolver el archivo PDF generado
                    return File(pdfBytes, "application/pdf", $"Factura_{factura.Factura.NumeroFactura}.pdf");
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                TempData["MensajeError"] = "Ocurrió un error al generar la factura: " + ex.Message;
                return RedirectToAction("HistorialCompras");
            }
        }


        // Acción para enviar la factura por correo electrónico
        public ActionResult EnviarFacturaCorreo(string numeroFactura, string emailDestino)
        {
            try
            {
                using (var context = new LaVentaMusicalEntities())
                {
                    // Obtener la factura y las relaciones de Usuario, DetalleFactura y Canciones
                    var factura = context.Facturas
                        .Where(f => f.NumeroFactura == numeroFactura)
                        .Select(f => new
                        {
                            Factura = f,
                            Usuario = f.Usuarios, // Obtener los datos del usuario
                            DetallesFactura = f.DetalleFactura.Select(d => new
                            {
                                Detalle = d,
                                Canciones = d.Canciones
                            }).ToList()
                        })
                        .FirstOrDefault();

                    // Verificar si la factura existe
                    if (factura == null)
                    {
                        return HttpNotFound("Factura no encontrada.");
                    }

                    // Obtener el correo electrónico del usuario
                    string usuarioEmail = factura.Usuario.Email; 


                    // Generar PDF de la factura
                    var pdfBytes = GenerarFacturaPDF(factura.Factura);

                    // Guardar el archivo temporalmente
                    var filePath = Path.GetTempFileName();
                    System.IO.File.WriteAllBytes(filePath, pdfBytes);

                    // Enviar el email con el archivo adjunto
                    using (var mensaje = new MailMessage("silvana@gmail.com", usuarioEmail)) // Usar el email del usuario
                    {
                        mensaje.Subject = "Factura de Compra";
                        mensaje.Body = "Adjuntamos la factura de tu compra en La Venta Musical.";
                        mensaje.Attachments.Add(new Attachment(filePath));

                        using (var cliente = new SmtpClient("smtp.gmail.com", 587)) // Servidor SMTP de Gmail
                        {
                            cliente.Credentials = new System.Net.NetworkCredential("silvana@gmail.com", "tu_password"); // Usar tus credenciales
                            cliente.EnableSsl = true;
                            cliente.Send(mensaje);
                        }
                    }

                    // Eliminar el archivo temporal después de enviarlo
                    System.IO.File.Delete(filePath);

                    // Establecer mensaje de éxito
                    TempData["Mensaje"] = "Factura enviada correctamente.";

                    // Redirigir al historial de compras
                    return RedirectToAction("HistorialCompras");
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                TempData["MensajeError"] = "Ocurrió un error al enviar la factura: " + ex.Message;
                return RedirectToAction("HistorialCompras");
            }
        }


        // Acción para mostrar los detalles de una factura
        public ActionResult FacturaDetalle(string numeroFactura)
        {
            try
            {
                using (var context = new LaVentaMusicalEntities())
                {
                    // Obtener la factura específica junto con sus detalles y el usuario
                    var factura = context.Facturas
                        .Where(f => f.NumeroFactura == numeroFactura)
                        .Select(f => new
                        {
                            Factura = f,
                            Usuario = f.Usuarios,
                            DetallesFactura = f.DetalleFactura.Select(d => new
                            {
                                Detalle = d,
                                Canciones = d.Canciones
                            }).ToList()
                        })
                        .FirstOrDefault();

                    // Verificar si la factura existe
                    if (factura == null)
                    {
                        return HttpNotFound("Factura no encontrada.");
                    }

                    // Mapear los datos a un ViewModel que será pasado a la vista
                    var model = new HistorialFacturaViewModel
                    {
                        NumeroFactura = factura.Factura.NumeroFactura,
                        FechaCompra = factura.Factura.FechaCompra ?? DateTime.MinValue,
                        Usuario = factura.Usuario.Nombre_Usuario,
                        SubTotal = factura.Factura.SubTotal ?? 0,
                        IVA = factura.Factura.IVA ?? 0,
                        Total = factura.Factura.Total,
                        Detalles = factura.DetallesFactura.Select(d => new FacturaDetalleViewModel
                        {
                            Cancion = d.Canciones.Nombre_Cancion,
                            PrecioUnitario = d.Detalle.PrecioUnitario ?? 0,
                            Cantidad = d.Detalle.Cantidad ?? 0,
                            Subtotal = d.Detalle.Subtotal ?? 0
                        }).ToList()
                    };

                    // Retornar la vista con el modelo
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                TempData["MensajeError"] = "Ocurrió un error al cargar los detalles de la factura: " + ex.Message;
                return RedirectToAction("HistorialCompras"); // Redirigir al historial de compras en caso de error
            }
        }

    }
}
