using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;


namespace Sistema_de_Facturacion.Models
{
    public class Clientes
    {
        [Key]
        public int IdCliente { get; set; }

        [MaxLength(20, ErrorMessage = "Debe contener como máximo 20 carácteres")]
        [Display(Name = "Nombre de cliente")]
        public string nombreCliente { get; set; }

        [MaxLength(30, ErrorMessage = "Debe contener como máximo 30 carácteres")]
        [Display(Name = "Razón social")]
        public string razonSocial { get; set; }

        [Display(Name = "RUT")]
        //[Range(12,12,ErrorMessage ="Debe contener 12 números")]
        public long rut { get; set; }

        [Display(Name = "Dirección")]
        public string direccion { set; get; }

        [Display(Name = "Estado de cuneta")]
        public double estadoCuenta { get; set; }

        public double Montofacturado { get; set; }

        public int IdUsuario { get; set; }
    }

   
}