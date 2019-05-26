using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sistema_de_Facturacion.Models;

namespace Sistema_de_Facturacion.Controllers
{
    public class ClientesController : Controller
    {

        

        public ClienteRepos _repos;
        public ClientesController()
        {
            _repos = new ClienteRepos();
        }

        // GET: Clientes
        public ActionResult Index()
        {
           
            if (Session["UsuarioId"] == null)
            {
                Session["NoSesion"]="debes iniciar sesión";
                return RedirectToAction("Index","Home",null);
            }
            int idUsaurio = (int)Session["UsuarioId"];
            var model = _repos.ObtenerTodos(idUsaurio);
            return View(model);
        }
        public JsonResult GetSearchValue2(string term)
        {
            int idUsuario = (int)Session["UsuarioId"];
            var allsearch = _repos.ObtenerClientes2(term,idUsuario);
            return Json(allsearch, JsonRequestBehavior.AllowGet);


        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idUsuario = (int)Session["UsuarioId"];
            var model = _repos.ObtenerCliente(id);
            string cliente = model.razonSocial;
            if (model == null)
            {
                return HttpNotFound();
            }
            
            string nrosFacturas = null;
            string nrosRecibos = null;
            double? plataFacturadaDolares=null;
            double? plataFacturadaPesos=null;
            double? pesos_a_dolares = null;
            double? TotalDolares_en_pesos = null;
            double? a_dolar = 0;
            List<string> error_tipo_cambio = null;
            ClienteRepos c = new ClienteRepos();
            List<DetalleFactura> clientesF = new List<DetalleFactura>();
            clientesF = c.DetalleCliente(id);
            
            for (int i = 0; i < clientesF.Count; i++)
            {
                nrosFacturas = nrosFacturas + "<li>" + clientesF[i].NroFactura + "</li>";
                if (clientesF[i].Moneda == "USS")
                {
                    double? totalD = clientesF[i].Total;
                    if (plataFacturadaDolares == null)
                    {
                        plataFacturadaDolares = totalD;
                    }
                    else
                    {
                        plataFacturadaDolares = plataFacturadaDolares + totalD;
                    }
                    pesos_a_dolares = clientesF[i].TotalPesos;

                    TotalDolares_en_pesos = TotalDolares_en_pesos + pesos_a_dolares;

                }
                else
                {
                    double? totalP = clientesF[i].TotalPesos;
                    if (plataFacturadaPesos == null)
                    {
                        plataFacturadaPesos = totalP;
                    }
                    else
                    {
                        plataFacturadaPesos = plataFacturadaPesos + totalP;
                    }
                    if (clientesF[i].TipoCambio == null)
                    {
                        
                        error_tipo_cambio.Add("La factura: " + clientesF[i].NroFactura + "no tiene tipo de cambio o no se pudo cargar de la base de datos");
                    }
                    a_dolar = a_dolar + (clientesF[i].TotalPesos / clientesF[i].TipoCambio);

                    
                   
                }
            }
            ViewBag.errorTipoCambio = error_tipo_cambio;
            ViewBag.nroFacturas = nrosFacturas;
            ViewBag.cantidadFacturas = clientesF.Count;
            ViewBag.facturadoUSD = plataFacturadaDolares;
            ViewBag.facturadoPesos = plataFacturadaPesos;
            if (plataFacturadaPesos != null)
            {
                if (pesos_a_dolares==null)
                {
                    pesos_a_dolares = 0;
                }
                double? total = (plataFacturadaDolares + pesos_a_dolares + a_dolar);
                ViewBag.TotalDolares = Math.Round((double)Convert.ToDouble(total),2);
            }
            else
            {
                ViewBag.TotalDolares = Math.Round((double)Convert.ToDouble(plataFacturadaDolares),2);
            }
            double? reciboUSD = null;
            double? reciboPesos = null;
            double? Total = 0;
            DetalleReciboRepos r = new DetalleReciboRepos();
            List<DetalleRecibo> recibos = new List<DetalleRecibo>();
            recibos = r.ObtenerRecibos(cliente,idUsuario);
            for (int j = 0; j < recibos.Count; j++)
            {
                nrosRecibos = nrosRecibos + "<li>" + recibos[j].NroRecibo + "</li>";
                if (recibos[j].Moneda == "U$S")
                {
                    double? totalDR = recibos[j].Monto;
                    if (reciboUSD == null)
                    {
                        reciboUSD = totalDR;
                    }
                    else
                    {
                        reciboUSD = reciboUSD + totalDR;
                    }

                }
                else
                {
                    double? totalPR = recibos[j].MontoPesos;
                    if (reciboPesos == null)
                    {
                        reciboPesos = totalPR;
                    }
                    else
                    {
                        reciboPesos = reciboPesos + totalPR;
                    }
                }
                Total = Total + recibos[j].Monto;
            }
            ViewBag.CantidadRecibos = recibos.Count;
            ViewBag.nroRecibos = nrosRecibos;
            ViewBag.ReciboDolares = reciboUSD;
            ViewBag.ReciboPesos = reciboPesos;
           
            double? totalR = (Total);
            ViewBag.TotalReciboUSD = Math.Round((double)Convert.ToDouble(totalR), 2);
               
     
                return View(model);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            if (Session["UsuarioId"] == null)
            {
                Session["NoSesion"] = "debes iniciar sesión";
                return RedirectToAction("Index", "Home", null);
            }
            return View();
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCliente,nombreCliente,razonSocial,rut,direccion,estadoCuenta")] Clientes cliente)
        {
            
            int idUsuario = (int)Session["UsuarioId"];
            if (ModelState.IsValid)
            {
                cliente.IdUsuario = idUsuario;
                _repos.CrearCliente(cliente);
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UsuarioId"] == null)
            {
                Session["NoSesion"] = "debes iniciar sesión";
                return RedirectToAction("Index", "Home", null);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = _repos.ObtenerCliente(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);

        }

        // POST: Clientes/Edit/5


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCliente,nombreCliente,razonSocial,rut,direccion,estadoCuenta")] Clientes cliente)
        {
            if (ModelState.IsValid)
            {

                _repos.EditarProducto(cliente);
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = _repos.ObtenerCliente(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repos.BorrarCliente(id);
            return RedirectToAction("Index");
        }
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repos.Disposing();
            }
            base.Dispose(disposing);
        }
    }
}
