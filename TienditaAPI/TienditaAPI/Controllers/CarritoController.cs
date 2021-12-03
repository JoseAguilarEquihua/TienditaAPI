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
using TienditaAPI.Models;

namespace TienditaAPI.Controllers
{
    [Authorize]
    public class CarritoController : ApiController
    {
        private TienditaEntities1 db = new TienditaEntities1();

        // GET: api/Carrito
        public IQueryable<Carrito> GetCarrito()
        {
            return db.Carrito;
        }

        // GET: api/Carrito/5
        [ResponseType(typeof(Carrito))]
        public IHttpActionResult GetCarrito(int id)
        {
            Carrito carrito = db.Carrito.Find(id);
            if (carrito == null)
            {
                return NotFound();
            }

            return Ok(carrito);
        }

        // PUT: api/Carrito/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCarrito(int id, Carrito carrito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carrito.IdCarrito)
            {
                return BadRequest();
            }

            db.Entry(carrito).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarritoExists(id))
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

        // POST: api/Carrito
        [ResponseType(typeof(Carrito))]
        public IHttpActionResult PostCarrito(Carrito carrito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!UsuarioHasCarrito(carrito.Correo))
            {
                db.Carrito.Add(carrito);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = carrito.IdCarrito }, carrito);
            }
            else
            {
                carrito = db.Carrito.SingleOrDefault(c  => c.Correo == carrito.Correo);
                return Ok(carrito);
            }
        }

        // DELETE: api/Carrito/5
        [ResponseType(typeof(Carrito))]
        public IHttpActionResult DeleteCarrito(int id)
        {
            Carrito carrito = db.Carrito.Find(id);
            if (carrito == null)
            {
                return NotFound();
            }

            db.Carrito.Remove(carrito);
            db.SaveChanges();

            return Ok(carrito);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarritoExists(int id)
        {
            return db.Carrito.Count(e => e.IdCarrito == id) > 0;
        }

        private bool UsuarioHasCarrito(string correo)
        {
            return db.Carrito.Count(e => e.Correo == correo) > 0;
        }
    }
}