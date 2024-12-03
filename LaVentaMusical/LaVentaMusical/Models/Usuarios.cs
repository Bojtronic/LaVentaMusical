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
    using System.ComponentModel.DataAnnotations;

    public partial class Usuarios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuarios()
        {
            this.Facturas = new HashSet<Facturas>();
            this.Tarjetas = new HashSet<Tarjetas>();
            this.Canciones = new HashSet<Canciones>();
        }

        [Display(Name = "#")]
        public int Id_Usuario { get; set; }
        [Display(Name = "Identificacion")]
        public string Identificacion { get; set; }
        [Display(Name = "Nombre_Usuario")]
        public string Nombre_Usuario { get; set; }
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }
        [Display(Name = "Genero")]
        public string Genero { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "TipoTarjeta")]
        public string TipoTarjeta { get; set; }
        [Display(Name = "NumeroTarjeta")]
        public string NumeroTarjeta { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Perfil")]
        public string Perfil { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Facturas> Facturas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tarjetas> Tarjetas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Canciones> Canciones { get; set; }
    }
}
