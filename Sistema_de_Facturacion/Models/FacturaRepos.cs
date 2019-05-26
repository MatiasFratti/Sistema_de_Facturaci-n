using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace Sistema_de_Facturacion.Models
{
    public class FacturaRepos
    {
        public object GetClient(string term)
        {
            using (var db = new DbSetContext())
            {

                var rutt = db.clientes.Where(x => x.razonSocial.Contains(term)).Select(x => new {
                    x.rut,
                    x.direccion

                    }).Take(1).ToList();
                return rutt;
            }
        }
        public Clientes GetIdClient(string term)
        {
            using (var db = new DbSetContext())
            {

                Clientes idc = db.clientes
                     .Where(b => b.razonSocial == term)
                     .FirstOrDefault();
                return idc;
            }
        }
        public List<Clientes> ObtenerClientesAuto(string term)
        {
            Clientes c = new Clientes();
            
            try
            {
                using (var db = new DbSetContext())
                {

                    var model= db.clientes.Where(x => x.nombreCliente.Contains(term)).Select(x => new Clientes {
                        rut= x.rut,
                        

                    }).Take(1).ToList();
                    return model;
                }
            }
            catch
            {
                return null;
            }


        }
        public void Agregar_detalle(int idU, int Idc, string productos, DateTime fecha/*int? dia, int? mes, double? anio*/, double? subtotal, double? total, string moneda, string nroFactura, string nombre, double? tipoCambio)
        {
            
            try
            {
               
                if (tipoCambio == null)
                {
                    throw new Exception("Error");

                }

            }
            catch(Exception ex)
            {
                ex.ToString();
            }
            
            DetalleFactura detalle = new DetalleFactura();

            detalle.IdUsuario = idU;
            detalle.Producto = productos;
            detalle.IdCliente = Idc;
            detalle.Fecha_emision =fecha;
            detalle.Moneda = moneda;
            detalle.Subtotal = subtotal;
            detalle.TipoCambio = tipoCambio;
            if (moneda == "USS")
            {
                detalle.Total = total;
            }
            else
            {
                detalle.Total = total / tipoCambio;
                detalle.TotalPesos = total;
            }
            
            
            detalle.NroFactura = nroFactura;
            detalle.Nombre = nombre;
            try
            {
                using (var db = new DbSetContext())
                {
                    db.DetalleFacturas.Add(detalle);
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                ex.ToString();
            }
        }
        public List<DetalleFactura> ObtenerTodos(int idUsuario)
        {
            using (var db = new DbSetContext())
            {
                return db.DetalleFacturas.Where(x => x.IdUsuario == idUsuario).ToList();
            }
        }
        public bool ActualizaStock(string[] productos, int[] cantidad)
        {
            
            using (var db = new DbSetContext())
            {
                for(var i=0; i < productos.Count(); i++)
                {
                    if (productos[i] != "")
                    {
                        string prod = productos[i];
                        int resultadoStock;
                        Productos actualizaStock = db.Productos
                             .Where(b => b.NombreProducto == prod)
                             .FirstOrDefault();
                        int primero = actualizaStock.Stock;
                        resultadoStock = primero - cantidad[i];
                        if (resultadoStock >= 0)
                        {
                            actualizaStock.Stock = resultadoStock;
                            try
                            {
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                ex.ToString();
                            }
                        }
                        else
                        {
                            return false;
                        }
                       
                    }
                }
               
            }
            return true;
        }
        public void ActualizaEstadoCuentaFactura(int idc, double plata, string moneda, double tipoCambio)
        {
            using(var db = new DbSetContext())
            {
                Clientes client = db.clientes.Where(c => c.IdCliente == idc).FirstOrDefault();
                if (moneda != "$")
                {

                    client.estadoCuenta = (client.estadoCuenta - plata);
                }
                else
                {
                    double estado = (client.estadoCuenta - (plata / tipoCambio));
                    client.estadoCuenta = Math.Round(estado, 2);
                }
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            
        }
        public void ActualizaEstadoCuentaRecibo(int idc,string moneda, double plata, double tipoCambio)
        {
            using (var db = new DbSetContext())
            {
                Clientes client = db.clientes.Where(c => c.IdCliente == idc).FirstOrDefault();
                if (moneda == "0")
                {
                    client.estadoCuenta = Math.Round((client.estadoCuenta + plata),2);
                }
                else
                {
                    
                    double estado = (client.estadoCuenta + plata/tipoCambio);
                    client.estadoCuenta = Math.Round(estado, 2);
                }
                
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }

        }

        public object GetFacturaForName(int id, string term)
        {
            using (var db = new DbSetContext())
            {

                var name = db.clientes.Where(x => x.razonSocial.Contains(term)&& x.IdUsuario==id).Select(x=> x.razonSocial).Take(6).ToList();
                return name;
            }
        }
        public object GetDetalleFactura(int id, string term)
        {
            using (var db = new DbSetContext())
            {
                var factura = db.DetalleFacturas.Where(x => x.Nombre == term && x.IdUsuario == id).ToList();
                return factura;
            }
        }
        public List<DetalleFactura> GetDetalleFactura2(int id, DateTime dateInit, DateTime dateFinal)
        {
            using (var db = new DbSetContext()) {
                
                var factura = db.DetalleFacturas.Where(x => x.IdUsuario == id && x.Fecha_emision>=dateInit && x.Fecha_emision<=dateFinal).ToList();
                
                       
                return factura;
            }

        }
        public List<DetalleFactura> GetDetalleFactura3(int id, DateTime dateInit, DateTime dateFinal, string Client)
        {
            using (var db = new DbSetContext())
            {

                var factura = db.DetalleFacturas.Where(x => x.IdUsuario == id && x.Fecha_emision >= dateInit && x.Fecha_emision <= dateFinal && x.Nombre == Client).ToList();


                return factura;
            }

        }
        public string ObtenerUltimaFactura(int idUsuario)
        {
            using (var db = new DbSetContext())
            {
                string nro="";
                var model =  db.DetalleFacturas.Where(x => x.IdUsuario == idUsuario).ToList();
                for(int i=0; i<model.Count();i++)
                {
                    nro = model[i].NroFactura;
                }
                return nro;
            }
        }
    }
}