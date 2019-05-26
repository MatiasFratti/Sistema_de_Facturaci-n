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
    public class ProductosController : Controller
    {
        private ProductoRepos _repos;
        public ProductosController()
        {
            _repos = new ProductoRepos();
        }
        private Productos p = new Productos();

        // GET: Productos
        public ActionResult Index()
        {
            try
            {
                
                if (Session["UsuarioId"] == null)
                {
                    Session["NoSesion"] = "debes iniciar sesión";
                    return RedirectToAction("Index", "Home", null);
                }
                int idUsuario = (int)Session["UsuarioId"];
                var model = _repos.ObtenerTodos(idUsuario);
                return View(model);
            }
            catch
            {
                return View("Index");
            }
        }
       
        public JsonResult GetSearchValue(string term)
        {
            int idUsuario = (int)Session["UsuarioId"];
            var allsearch = _repos.ObtenerProducto2(term, idUsuario);
            return Json(allsearch, JsonRequestBehavior.AllowGet);
            
            
        }
        public JsonResult MultiplicaCantidadPrecio(string term)
        {

            var allsearch = _repos.ObtenerPrecio(term);
            return Json(allsearch, JsonRequestBehavior.AllowGet);


        }

        // GET: Productos/Details/5
        public ActionResult Details(int? id)
       {
            try
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
                var model = _repos.ObtenerProducto(id);
                if (model == null)
                {
                    return HttpNotFound();
                }
                return View(model);
            }
            catch
            {
                return View("Index");
            }
        }

        // GET: Productos/Create
        public ActionResult Create()
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
                return View("Index");
            }
        }

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NombreProducto,Precio,Categoria,Stock")] Productos productos)
        {
            try
            {
                int idUsuario = (int)Session["UsuarioId"];

                if (ModelState.IsValid)
                {   
                    _repos.CrearProducto(productos,idUsuario);
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("Create");
            }

            return View(productos);
        }

        // GET: Productos/Edit/5
        public ActionResult Edit(int? id)
        {
            try
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
                var model = _repos.ObtenerProducto(id);
                if (model == null)
                {
                    return HttpNotFound();
                }
                return View(model);
            }
            catch
            {
                return View("Index");
            }
        }

        // POST: Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NombreProducto,Precio,Categoria,Stock")] Productos productos)
        {
            try
            {
                int idUsuario = (int)Session["UsuarioId"];
                productos.IdUsuario = idUsuario;
                if (ModelState.IsValid)
                {
                    _repos.EditarProducto(productos);
                    return RedirectToAction("Index");
                }
                return View(productos);
            }
            catch
            {
                return View("Index");
            }
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            try
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
                var model = _repos.ObtenerProducto(id);
                if (model == null)
                {
                    return HttpNotFound();
                }
                return View(model);
            }
            catch
            {
                return View("Index");
            }
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (Session["UsuarioId"] == null)
                {
                    Session["NoSesion"] = "debes iniciar sesión";
                    return RedirectToAction("Index", "Home", null);
                }
                _repos.BorrarProducto(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Index");
            }
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
