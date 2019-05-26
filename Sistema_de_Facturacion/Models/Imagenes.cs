using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sistema_de_Facturacion.Models
{
    public class Imagenes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdImagen { get; set; }

        public string Nombre { get; set; }

        public string Extension { get; set; }

        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuarios Usuarios { get; set; }

        public void GuardarImagen(Imagenes img)
        {
            using (var db = new DbSetContext())
            {
                db.Imagen.Add(img);
                db.SaveChanges();

            }
        }

        public void ActualizarImagen(Imagenes img)
        {
            using (var db = new DbSetContext())
            {
                db.Entry(img).State = EntityState.Modified;
                db.SaveChanges();

            }
        }

        public Imagenes GetImagen(int? id)
        {
            using (var db = new DbSetContext())
            {
                var imagen = db.Imagen.Where(x => x.IdUsuario == id).First();

                return imagen;
            }
        }
    }

   
}