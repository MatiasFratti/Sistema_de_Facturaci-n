using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistema_de_Facturacion.Models
{
    public class DetalleRecibo
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="N° de Recibo")]
        public string NroRecibo { get; set; }
        [Display(Name = "N° de Facturas")]
        public string NroFactura { get; set; }

        public string Cliente { get; set; }

        public string Moneda { get; set; }
        [Display(Name = "Tipo de Pago")]
        public string TipoPago { get; set; }

        [Display(Name = "N° de Cheque")]
        public long? NroCheque { get; set; }
        [Display(Name = "Pago")]
        public double Monto { get; set; }

        public double? TipoCambio { get; set; }

        public double? MontoPesos { get; set; }

        public string Banco { get; set; }

        public string Fecha { get; set; }

        public int IdUsuario { get; set; }


    }
}