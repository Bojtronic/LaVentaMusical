//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LaVentaMusical.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Canciones
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Canciones()
        {
            this.DetalleFactura = new HashSet<DetalleFactura>();
            this.Facturas = new HashSet<Facturas>();
            this.Usuarios = new HashSet<Usuarios>();
        }
    
        public int Id_Cancion { get; set; }
        public int Id_Genero { get; set; }
        public int Id_Album { get; set; }
        public string Nombre_Cancion { get; set; }
        public string Video_URL { get; set; }
        public Nullable<decimal> Precio { get; set; }
        public Nullable<int> Canciones_Disponibles { get; set; }
    
        public virtual Albumes Albumes { get; set; }
        public virtual Generos Generos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleFactura> DetalleFactura { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Facturas> Facturas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Usuarios> Usuarios { get; set; }
    }
}
