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
    public class DetallePedidoController : ApiController
    {
        private TienditaEntities1 db = new TienditaEntities1();

        // GET: api/DetallePedido
        public IQueryable<DetallePedido> GetDetallePedido()
        {
            return db.DetallePedido;
        }

        // GET: api/DetallePedido/5
        [ResponseType(typeof(DetallePedido))]
        public IHttpActionResult GetDetallePedido(int id)
        {
            List<DetallePedido> detallePedido = db.DetallePedido.Where(d => d.IdPedido == id).ToList();
            if (detallePedido == null)
            {
                return NotFound();
            }

            return Ok(detallePedido);
        }
     
        // PUT: api/DetallePedido/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDetallePedido(int id, DetallePedido detallePedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != detallePedido.Id)
            {
                return BadRequest();
            }

            db.Entry(detallePedido).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetallePedidoExists(id))
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

        // POST: api/DetallePedido
        [ResponseType(typeof(DetallePedido))]
        public IHttpActionResult PostDetallePedido(DetallePedido detallePedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DetallePedido.Add(detallePedido);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = detallePedido.Id }, detallePedido);
        }

        // DELETE: api/DetallePedido/5
        [ResponseType(typeof(DetallePedido))]
        public IHttpActionResult DeleteDetallePedido(int id)
        {
            DetallePedido detallePedido = db.DetallePedido.Find(id);
            if (detallePedido == null)
            {
                return NotFound();
            }

            db.DetallePedido.Remove(detallePedido);
            db.SaveChanges();

            return Ok(detallePedido);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DetallePedidoExists(int id)
        {
            return db.DetallePedido.Count(e => e.Id == id) > 0;
        }
    }
}