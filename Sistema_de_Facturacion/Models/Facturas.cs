using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_de_Facturacion.Models
{
    public class Facturas
    {
        [Key]
        public int IdFactura { get; set; }

        public long NroFactura { get; set; }
      
        public string fecha { get; set; }

    }
}