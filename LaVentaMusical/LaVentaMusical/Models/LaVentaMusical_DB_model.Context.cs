﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LaVentaMusicalEntities : DbContext
    {
        public LaVentaMusicalEntities()
            : base("name=LaVentaMusicalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Albumes> Albumes { get; set; }
        public virtual DbSet<Artistas> Artistas { get; set; }
        public virtual DbSet<Canciones> Canciones { get; set; }
        public virtual DbSet<DetalleFactura> DetalleFactura { get; set; }
        public virtual DbSet<Facturas> Facturas { get; set; }
        public virtual DbSet<Generos> Generos { get; set; }
        public virtual DbSet<Pagos> Pagos { get; set; }
        public virtual DbSet<Tarjetas> Tarjetas { get; set; }
        public virtual DbSet<Tipo_Tarjeta> Tipo_Tarjeta { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
    }
}
