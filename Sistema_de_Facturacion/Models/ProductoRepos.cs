
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sistema_de_Facturacion.Models
{
    public class ProductoRepos
    {
        public List<Productos> ObtenerTodos(int idUsuario)
        {
            using (var db = new DbSetContext())
            {
                return db.Productos.Where(x =>x.IdUsuario==idUsuario).ToList();
            }
        }
        public Productos ObtenerProducto(int? id)
        {
            using(var db = new DbSetContext())
            {
                return db.Productos.Find(id);
            }
        }
        public void CrearProducto(Productos productos,int idUsuario)
        {
            using (var db = new DbSetContext())
            {
                productos.IdUsuario = idUsuario;
                db.Productos.Add(productos);
                db.SaveChanges();
                
            }
        }
        public void  EditarProducto(Productos productos)
        {
            using (var db = new DbSetContext())
            {
                db.Entry(productos).State = EntityState.Modified;
                db.SaveChanges();
               
            }
        }
        public void BorrarProducto(int id)
        {
            using(var db = new DbSetContext())
            {
                var model = db.Productos.Find(id);
                db.Productos.Remove(model);
                db.SaveChanges();
            }
        }
        public void Disposing()
        {
            using(var db = new DbSetContext())
            {
                db.Dispose();
            }
        }
        public List<string> ObtenerProducto2(string term, int id)
        {
            using (var db = new DbSetContext())
            {

                return db.Productos.Where(x => x.NombreProducto.Contains(term) && x.IdUsuario==id).Select(x => x.NombreProducto).Take(6).ToList();

            }
        }
        public object ObtenerPrecio(string term)
        {
            using (var db = new DbSetContext())
            {

                return (db.Productos.Where(x => x.NombreProducto.Contains(term)).Select(x => new { x.Precio }).Take(1).ToList());

            }
        }
    }
}