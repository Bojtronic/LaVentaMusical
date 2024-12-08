using System;
using System.Collections.Generic;

namespace LaVentaMusical.Models
{
    public class FacturaDetalleViewModel
    {
        public string Cancion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class HistorialFacturaViewModel
    {
        public string NumeroFactura { get; set; }
        public DateTime FechaCompra { get; set; }
        public string Usuario { get; set; }
        public decimal SubTotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public List<FacturaDetalleViewModel> Detalles { get; set; }
    }
}
