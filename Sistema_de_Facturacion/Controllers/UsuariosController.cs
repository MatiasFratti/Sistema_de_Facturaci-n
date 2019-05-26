using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Sistema_de_Facturacion.Models;
using System.IO;

namespace Sistema_de_Facturacion.Controllers
{
    public class UsuariosController : Controller
    {
        private UsuarioRepos _repos;
        public UsuariosController()
        {
            _repos = new UsuarioRepos();
        }

        public ActionResult Registro()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Registro(Usuarios usr)
        {
            var imagen = new Imagenes();
            if (ModelState.IsValid)
            {
                _repos.Registrarse(usr);
                var usuario = _repos.GetUsuario(usr.Nombre, usr.Contraseña);
                imagen.IdUsuario = usuario.Id;
                imagen.Nombre = "http://www.aesoftmarket.com/uploads/fe-logo-8076354209.png";
                imagen.Extension = "imagen/png";
                imagen.GuardarImagen(imagen);
                
                ModelState.Clear();
                
            }

            return View("Logearse");
        }

        // GET: Usuarios
        public ActionResult Logearse()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Logearse(Usuarios usr)
        {         
            var usuario = _repos.logeo(usr);

            if (usuario != null)
            {
                Session["UsuarioId"] = usuario.Id;
                Session["UsuarioRazon"] = usuario.RazonSocial;
                Session["UsuarioRut"] = usuario.Rut;
                Session["UsuarioDire"] = usuario.Direccion;
                Session["UsuarioTel"] = usuario.Telefono;



                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.error = "Error de credenciales, intentelo nuevamente";
            }
       
            return View();


        }


        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = _repos.GetUsuario(id);
            Imagenes img = new Imagenes();
            var imagen = img.GetImagen(id);
            if(imagen == null)
            {
                return View(usuarios);
            }
            ViewBag.img = imagen.Nombre;
            

            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }



        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = new Usuarios();
            usuarios = _repos.GetUsuario(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Nombre,RazonSocial,Rut,Direccion,Telefono,Contraseña")] Usuarios usuarios)
        {
            if (usuarios.Nombre!=null && usuarios.RazonSocial!=null && usuarios.Rut!=0 && usuarios.Direccion!= null && usuarios.Telefono!=0 && usuarios.Contraseña!= null)
            {
                int idUsuario = (int)Session["UsuarioId"];
                usuarios.Id = idUsuario;
                usuarios.ConfirmarContraseña = usuarios.Contraseña;
                _repos.Editar(usuarios);
                ViewBag.msj = "Usuario actualizado correctamente";
                return View(usuarios);
            }
            return View(usuarios);
        }
        [HttpPost]
        public ActionResult SubirImagen(HttpPostedFileBase file)
        {
            int idUsuario = (int)Session["UsuarioId"];
            if(file!=null && file.ContentLength > 0)
            {
                Imagenes imagen = new Imagenes();
                var img = imagen.GetImagen(idUsuario);
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Context"), fileName);
                
                file.SaveAs(path);
                img.Nombre = fileName;
                img.Extension = file.ContentType;
                img.IdUsuario = idUsuario;
                img.ActualizarImagen(img);
            }
            return Redirect("Details/" + idUsuario);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //       _repos.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
    
