using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using TienditaAPI.Models;

namespace TienditaAPI.Controllers
{
    public class UsuariosController : ApiController
    {
        private TienditaEntities db = new TienditaEntities();

        // GET: api/Usuarios
        public IQueryable<Usuario> GetUsuario()
        {
            return db.Usuario;
        }

        // GET: api/Usuarios/5
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult GetUsuario(string id)
        {
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult Index(string correo, string contrasenia)
        {
            try
            {
                using (Models.TienditaEntities db = new Models.TienditaEntities())
                {
                    var isUser = (from t in db.Usuario
                                  where t.Correo == correo.Trim() && t.Contrasenia == contrasenia.Trim()
                                  select t).FirstOrDefault();
                    if (isUser == null)
                    {                       
                        return NotFound();
                    }
                    
                }
                return Ok();

            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        // GET: api/Usuarios/correo/password
        //POST api/PreviewLCAPI
        //[Route("")]
        //[HttpPost]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Usuarios/Login")]
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult Login(AuthModel login)
        {
            bool usuario = UsuarioLogin(login.Correo, login.Contrasenia);
            if (usuario)
            {                
                return Ok(db.Usuario.Find(login.Correo));
            }
            else
            {
                return NotFound();
            }
            

        }

        // PUT: api/Usuarios/5        
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsuario(string id, Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.Correo)
            {
                return BadRequest();
            }

            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Usuarios
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult PostUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Usuario.Add(usuario);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UsuarioExists(usuario.Correo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = usuario.Correo }, usuario);
        }

        // DELETE: api/Usuarios/5
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult DeleteUsuario(string id)
        {
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.Usuario.Remove(usuario);
            db.SaveChanges();

            return Ok(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioExists(string id)
        {
            return db.Usuario.Count(e => e.Correo == id) > 0;
        }

        private bool UsuarioLogin(string id, string password)
        {
            return  db.Usuario.Count(e => e.Correo == id && e.Contrasenia == password) > 0;            
        }
    }
}