using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistema_de_Facturacion.Models
{
    public class DetalleReciboRepos
    {
        public List<DetalleRecibo> ObtenerTodos(int idUsuario)
        {
            using (var db = new DbSetContext())
            {
                return db.DetalleRecibos.Where(x => x.IdUsuario == idUsuario).ToList();
            }
        }
        public bool AgregarDetalleRecibo(DetalleRecibo recibo, int idUsuario)
        {
            try
            {
                
                using (var db = new DbSetContext())
                {
                    recibo.IdUsuario = idUsuario;
                    if(recibo.Moneda == "U$S")
                    {
                        recibo.MontoPesos = 0;
                    }
                    if(recibo.Moneda == "$")
                    {
                        recibo.MontoPesos = recibo.Monto;
                        recibo.Monto = Math.Round((double)Convert.ToDouble(recibo.Monto/recibo.TipoCambio),2);
                         
                    }
                    db.DetalleRecibos.Add(recibo);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public List<DetalleRecibo> ObtenerRecibos(string cliente,int idUsuario)
        {
            using(var db = new DbSetContext())
            {
                List<DetalleRecibo> recibos = db.DetalleRecibos.Where(r => r.Cliente == cliente && r.IdUsuario == idUsuario).ToList();
                return recibos;
            }
            
        }
    }
}