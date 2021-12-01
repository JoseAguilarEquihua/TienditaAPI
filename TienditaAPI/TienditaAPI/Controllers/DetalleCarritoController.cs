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
    public class DetalleCarritoController : ApiController
    {
        private TienditaEntities1 db = new TienditaEntities1();

        // GET: api/DetalleCarrito
        public IQueryable<DetalleCarrito> GetDetalleCarrito()
        {
            return db.DetalleCarrito;
        }

        // GET: api/DetalleCarrito/5
        [ResponseType(typeof(DetalleCarrito))]
        public IHttpActionResult GetDetalleCarrito(int id)
        {
            List<DetalleCarrito> detalles = db.DetalleCarrito.Where(d => d.IdCarrito == id).ToList();
            //List<DetalleCarrito> detalleCarrito = db.DetalleCarrito.Where(c => c.IdCarrito == id); 
            if (detalles == null)
            {
                return NotFound();
            }

            return Ok(detalles);
        }

        // PUT: api/DetalleCarrito/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDetalleCarrito(int id, DetalleCarrito detalleCarrito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != detalleCarrito.Id)
            {
                return BadRequest();
            }

            db.Entry(detalleCarrito).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetalleCarritoExists(id))
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

        // POST: api/DetalleCarrito
        [ResponseType(typeof(DetalleCarrito))]
        public IHttpActionResult PostDetalleCarrito(DetalleCarrito detalleCarrito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!ProductExists(detalleCarrito.IdProducto, detalleCarrito.IdCarrito)){
                db.DetalleCarrito.Add(detalleCarrito);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = detalleCarrito.Id }, detalleCarrito);
            } else
            {
                detalleCarrito = db.DetalleCarrito.SingleOrDefault(e => e.IdProducto == detalleCarrito.IdProducto && e.IdCarrito == detalleCarrito.IdCarrito);
                return Ok(detalleCarrito);
            }

            



            
        }

        // DELETE: api/DetalleCarrito/5
        [ResponseType(typeof(DetalleCarrito))]
        public IHttpActionResult DeleteDetalleCarrito(int id)
        {
            DetalleCarrito detalleCarrito = db.DetalleCarrito.Find(id);
            if (detalleCarrito == null)
            {
                return NotFound();
            }

            db.DetalleCarrito.Remove(detalleCarrito);
            db.SaveChanges();

            return Ok(detalleCarrito);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DetalleCarritoExists(int id)
        {
            return db.DetalleCarrito.Count(e => e.Id == id) > 0;
        }

        private bool ProductExists(int IdProducto, int IdCarrito)
        {
            return db.DetalleCarrito.Count(e => e.IdProducto == IdProducto && e.IdCarrito == IdCarrito) > 0;
        }
    }
}