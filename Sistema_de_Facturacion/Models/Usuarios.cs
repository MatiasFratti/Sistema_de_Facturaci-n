using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace Sistema_de_Facturacion.Models
{
    public class Usuarios
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(8, ErrorMessage = "Maximo 8 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(8, ErrorMessage = "Maximo 8 caracteres")]
        [Display(Name = "Contraseña")]
        public string Contraseña { get; set; }

        [NotMapped]
        [Display(Name ="Confirmar contraseña")]
        [MaxLength(8, ErrorMessage = "Maximo 8 caracteres")]
        [Compare("Contraseña", ErrorMessage ="La contraseña no coincide")]
        public string ConfirmarContraseña {get; set;}

        [Display(Name = "Razon Social")]
        public string RazonSocial { get; set; }

        
        public long Rut { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        public long Telefono { get; set; }
        

    }
    public class LoginUsuario
    {
        [Required]
        [MaxLength(8, ErrorMessage = "Maximo 8 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        [Required]
        [MaxLength(8, ErrorMessage = "Maximo 8 caracteres")]
        [Display(Name = "Contraseña")]
        public string Contraseña { get; set; }
    }
}

   