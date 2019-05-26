using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sistema_de_Facturacion.Models
{
    public class DetalleFactura
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDetalle { get; set; }
       
        public int? IdFactura { get; set; }

        [Display(Name = "Serie y N° de facturas")]
        public string NroFactura { get; set; }

        [Display(Name = "Nombre Cliente")]
        public string Nombre { get; set; }


        [ForeignKey("IdFactura")]
        public Facturas Facturas { get; set; }
        
        [Display(Name ="Código cliente")]
        public int? IdCliente { get; set; }

        [ForeignKey("IdCliente")]
        public Clientes Clientes { get; set; }

        [Display(Name = "Producto")]
        public string Producto { get; set; }

        [NotMapped]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Display(Name = "Moneda")]
        public string Moneda { get; set; }

        public double? TipoCambio { get; set; }


        [Display(Name = "Subtotal")]
        public double? Subtotal { get; set; }

        [Display(Name = "Total + IVA")]
        public double? Total { get; set; }

        public double?  TotalPesos { get; set; }

        [Display(Name = "Fecha de emisión")]
        public DateTime Fecha_emision { get; set; }

        public int IdUsuario { get; set; }
    }
}