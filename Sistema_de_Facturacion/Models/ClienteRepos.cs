using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sistema_de_Facturacion.Models
{
    public class ClienteRepos
    {
        public List<Clientes> ObtenerTodos(int idUsuario)
        {
            using (var db = new DbSetContext())
            {
                return db.clientes.Where(x => x.IdUsuario == idUsuario).ToList();
            }
        }
        public Clientes ObtenerCliente(int? id)
        {
            using (var db = new DbSetContext())
            {
                return db.clientes.Find(id);
            }
        }
        public void CrearCliente(Clientes cliente)
        {
            using (var db = new DbSetContext())
            {

                db.clientes.Add(cliente);
                db.SaveChanges();

            }
        }
        public void EditarProducto(Clientes cliente)
        {
            using (var db = new DbSetContext())
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();

            }
        }
        public void BorrarCliente(int id)
        {
            using (var db = new DbSetContext())
            {
                var model = db.clientes.Find(id);
                db.clientes.Remove(model);
                db.SaveChanges();
            }
        }
       
        public List<string> ObtenerClientes2(string term, int id)
        {
            try
            {
                using (var db = new DbSetContext())
                {

                    return db.clientes.Where(x => x.razonSocial.Contains(term) && x.IdUsuario==id).Select(x => x.razonSocial).Take(6).ToList();

                }
            }
            catch
            {
                return null;
            }


        } 

        public List<DetalleFactura> DetalleCliente(int? id)
        {
            using(var db = new DbSetContext())
            {
                try
                {
                    List<DetalleFactura> facturas = db.DetalleFacturas.Where(f => f.IdCliente == id).ToList();
                    return facturas;
                }
                catch
                {
                    return null;
                }
            }
           
        }


        public void Disposing()
        {
            using (var db = new DbSetContext())
            {
                db.Dispose();
            }
        }
    }
}