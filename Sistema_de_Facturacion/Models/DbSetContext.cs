using Sistema_de_Facturacion.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sistema_de_Facturacion.Models
{
    public class DbSetContext:DbContext
    {
        public DbSetContext()
           :base("DefaultConnection")
        { 

        }

        public DbSet<Productos> Productos { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Clientes> clientes { get; set; }
        public DbSet<Facturas> Facturas { get; set; }
        public DbSet<DetalleFactura> DetalleFacturas { get; set; }
        public DbSet<DetalleRecibo> DetalleRecibos { get; set; }
        public DbSet<Imagenes> Imagen { get; set; }
    }
}