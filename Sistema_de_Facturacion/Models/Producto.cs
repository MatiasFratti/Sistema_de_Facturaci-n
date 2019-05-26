using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace Sistema_de_Facturacion.Models
{
    public class Productos
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Producto")]
        public string NombreProducto { get; set; }

        [Display(Name = "Precio")]
        public double Precio { get; set; }

        [Display(Name = "Categoria")]
        public string Categoria { get; set; }

        [Required]
        [Display(Name = "Stock")]
        public int Stock { get; set; }

        public int IdUsuario { get; set; }
         
    }
}