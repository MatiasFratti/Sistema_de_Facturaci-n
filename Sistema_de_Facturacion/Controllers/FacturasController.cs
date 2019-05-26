using Sistema_de_Facturacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Rotativa;

namespace Sistema_de_Facturacion.Controllers
{
    public class FacturasController : Controller
    {
       

        private FacturaRepos _repos;
        private DetalleReciboRepos _rep;

        public FacturasController()
        {
            _repos = new FacturaRepos();
            _rep = new DetalleReciboRepos();
        }
       
        // GET: Facturas
        public ActionResult Facturar()
        {
            try
            {
                if (Session["UsuarioId"] == null)
                {
                    Session["NoSesion"] = "debes iniciar sesión";
                    return RedirectToAction("Index", "Home", null);
                }
                int idUsuario = (int)Session["UsuarioId"];
                
                
                ViewBag.Razon = (string)Session["UsuarioRazon"];
                ViewBag.Rut = (long)Session["UsuarioRut"];
                ViewBag.Dire = (string)Session["UsuarioDire"];
                ViewBag.Tel = (long)Session["UsuarioTel"];

                Imagenes img = new Imagenes();
                var imagen = img.GetImagen(idUsuario);

                if(imagen == null)
                {
                    return View();
                }
                ViewBag.logo = imagen.Nombre;
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home", null);
            }
        }
        public ActionResult AutoCLiente(string term)
        {
            var model= _repos.GetClient(term);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Facturar(string nombre, string[] productos, int[] cantidad, DateTime fecha /*int? dia, int? mes, double? anio*/, string subtotal, string total, string moneda, string tipo_cambio, string nroFactura)
        {
            try
            {
                int idUsuario = (int)Session["UsuarioId"];


                if (tipo_cambio == "")
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
              
                double Subtotal = (double.Parse(subtotal));
                double Total = (double.Parse(total));
                double Tc = double.Parse(tipo_cambio)/10;
               
                Clientes Idcient = new Clientes();
                Idcient = _repos.GetIdClient(nombre);
                int Idc = Idcient.IdCliente;

                string product = null;

                for (var i = 0; i < productos.Count(); i++)
                {
                    if (productos[i] != "")
                    {
                        product = product + "<li>" + cantidad[i] + " unidades de " + productos[i] + "</li>";

                    }
                }
               
                FacturaRepos f = new FacturaRepos();
                if(f.ActualizaStock(productos, cantidad)==true)
                {
                    _repos.Agregar_detalle(idUsuario, Idc, product,fecha /*dia, mes, anio*/, (Subtotal / 100), (Total / 100), moneda, nroFactura, nombre,Tc);
                    _repos.ActualizaEstadoCuentaFactura(Idc, Total/100,moneda,Tc);
                }
                else
                {
                ViewBag.error = "No hay stock de algún producto, por lo cual no se emitió la factura";
                return View();
                }

                ViewBag.nombre = nombre;
                ViewBag.nroFactura = nroFactura;
                ViewBag.producto = product;
                ViewBag.fecha = fecha;
                ViewBag.moneda = moneda;
                ViewBag.subtotal = subtotal;
                ViewBag.total = total;

                return View("DetalleFactura");
            }
            catch
            {
                ViewBag.error = "No se emotió la factura";
                return View();
            }
        }
      
        public ActionResult DetalleFacturas(int? cantidad_facturas)
        {
            try
            {
                var model = new List<DetalleFactura>();
                
                if (Session["UsuarioId"] == null)
                {
                    Session["NoSesion"] = "debes iniciar sesión";
                    return RedirectToAction("Index", "Home", null);
                }
                int idUsuario = (int)Session["UsuarioId"];
                if (cantidad_facturas == null) {
                    model = _repos.ObtenerTodos(idUsuario);
                }
                

                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "Home", null);
            }  
        }
        [HttpPost]
        public ActionResult DetalleFacturas(string nombreCliente, DateTime fecha)
        {

            return View();
        }

        public ActionResult ReciboDePago()
        {
            try
            {
                if (Session["UsuarioId"] == null)
                {
                    Session["NoSesion"] = "debes iniciar sesión";
                    return RedirectToAction("Index", "Home", null);
                }
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home", null);
            }
        }

        [HttpPost]
        public ActionResult ReciboDePago(string nro_recibo, string[] nro_factura, string moneda, string monto, string nombre_cliente, string tipo_pago, long? nro_cheque, string banco, string dia_mes,string anio,string Tipo_cambio)
        {
            try
            {
                int idUsuario = (int)Session["UsuarioId"];
                // (moneda){ 0  = U$S : 1 = $ }
                // (tipo de pago){ 0  = cheque : 1 = efectivo }
                double _monto = double.Parse(monto);
                string fecha = dia_mes + " " + anio;
                double Tc = double.Parse(Tipo_cambio) / 10;
                Clientes Idcient = new Clientes();
                Idcient = _repos.GetIdClient(nombre_cliente);
                int Idc = Idcient.IdCliente;

                string NroFact = null;
                DetalleRecibo r = new DetalleRecibo();
                r.NroRecibo = nro_recibo;
                if (moneda == "0") { r.Moneda = "U$S"; }
                if (moneda == "1") { r.Moneda = "$"; }
                r.Monto = _monto;
                r.Cliente = nombre_cliente;
                if (tipo_pago == "0")
                {
                    r.TipoPago = "Cheque";
                }
                if (tipo_pago == "1")
                {
                    r.TipoPago = "Efectivo";
                }
                r.NroCheque = nro_cheque;
                r.Banco = banco;
                for (var i = 0; i < nro_factura.Count(); i++)
                {
                    if (nro_factura[i] != "")
                    {
                        NroFact = NroFact + "<li>" + nro_factura[i] + "</li>";

                    }
                }
                r.NroFactura = NroFact;
                r.Fecha = fecha;
                r.TipoCambio = Tc;
                _rep.AgregarDetalleRecibo(r,idUsuario);
                _repos.ActualizaEstadoCuentaRecibo(Idc, moneda, _monto, Tc);

                return RedirectToAction("DetalleRecibos");
            }
            catch
            {
                return RedirectToAction("Index", "Home", null);
            }
        }

        public ActionResult DetalleRecibos()
        {
            try
            {
                if (Session["UsuarioId"] == null)
                {
                    Session["NoSesion"] = "debes iniciar sesión";
                    return RedirectToAction("Index", "Home", null);
                }
                int idUsuario = (int)Session["UsuarioId"];
                var model = _rep.ObtenerTodos(idUsuario);
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "Home", null);
            }
        }
        public ActionResult PDF_Factura()
        {
            int idUsuario = (int)Session["UsuarioId"];

            return new ViewAsPdf("Facturar")
            {
                //FileName = Server.MapPath("~/Context/Factura.pdf")
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4
            };
        }
        public ActionResult SelectForName(string term)
        {
            int idUsuario = (int)Session["UsuarioId"];
            var name = _repos.GetFacturaForName(idUsuario, term);
            return Json(name, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchFactura(string term)
        {
            int idUsuario = (int)Session["UsuarioId"];
            var model = _repos.GetDetalleFactura(idUsuario,term);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectForDate(string term, string term2)
        {
            
            string dateInit = term;
            DateTime parsedDateInit = DateTime.Parse(dateInit);
            string dateFinal = term2;
            DateTime parsedDateFinal = DateTime.Parse(dateFinal);
            if (parsedDateInit > parsedDateFinal)
            {
                return null;
            }
            int idUsuario = (int)Session["UsuarioId"];
            var model = _repos.GetDetalleFactura2(idUsuario, parsedDateInit, parsedDateFinal);
            if (model.Count() != 0)
            {
                ViewBag.fecha = model[0].Fecha_emision.Date;
            }
            
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NroFactura()
        {
            int idUsuario = (int)Session["UsuarioId"];
            var model = _repos.ObtenerUltimaFactura(idUsuario);
            
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectForDateAndCLient(string term, string term2, string term3)
        {
            string dateInit = term;
            DateTime parsedDateInit = DateTime.Parse(dateInit);
            string dateFinal = term2;
            DateTime parsedDateFinal = DateTime.Parse(dateFinal);

            if (parsedDateInit > parsedDateFinal)
            {
                return null;
            }

            int idUsuario = (int)Session["UsuarioId"];
            var model = _repos.GetDetalleFactura3(idUsuario, parsedDateInit, parsedDateFinal, term3);
            if(model.Count == 0)
            {
                return null;
            }
            ViewBag.fecha = model[0].Fecha_emision.Date;
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
