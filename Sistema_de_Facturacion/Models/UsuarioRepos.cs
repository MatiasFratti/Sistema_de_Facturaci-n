using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sistema_de_Facturacion.Models
{
    public class UsuarioRepos
    {
        public Usuarios logeo(Usuarios usuario)
        {
            try
            {
                using (DbSetContext db = new DbSetContext())
                {
                    var usr = db.Usuarios.Single(x => x.Nombre == usuario.Nombre && x.Contraseña==usuario.Contraseña);
                  

                    return usr;
                }
            }
            catch
            {
                return null;
            }
        }
        public void Registrarse(Usuarios usuario)
        {
            try
            {
                using (DbSetContext db = new DbSetContext())
                {

                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                }
            }
            catch
            {
                
            }
        }
        public void Editar(Usuarios usuario)
        {
            try
            {
                using (DbSetContext db = new DbSetContext())
                {

                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch
            {
                
            }
        }
        public Usuarios GetUsuario(int? id)
        {
            try
            {
                using (DbSetContext db = new DbSetContext())
                {
                    Usuarios usu = db.Usuarios.Find(id);
                    return usu;
                }
            }
            catch
            {
                return null;
            }
           
        }
        public Usuarios GetUsuario(string user, string pass)
        {
            try
            {
                using (DbSetContext db = new DbSetContext())
                {
                    var usu = db.Usuarios.Where(x => x.Nombre == user && x.Contraseña == pass).First();
                    return usu;
                }
            }
            catch
            {
                return null;
            }

        }
    }
}