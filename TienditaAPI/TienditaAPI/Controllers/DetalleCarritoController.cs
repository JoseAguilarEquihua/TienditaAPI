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
    public class DetalleCarritoController : ApiControlle
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

        //[System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/DetalleCarrito/GetCarrito/{id}")]
        [ResponseType(typeof(DetalleCarritoProductoModel))]
        public IHttpActionResult GetDetalleCarritoProducto(int id)
        {
            //DetalleCarritoProductoModel detalleCarritoProductoModel = new DetalleCarritoProductoModel();            
            List<DetalleCarritoProductoModel> detalles = db.DetalleCarrito.Join(db.Producto, det => det.IdProducto, prod => prod.IdProducto,
                (det, prod) => new DetalleCarritoProductoModel
                {
                    Producto = prod.Producto1,
                    Cantidad = det.Cantidad,
                    Costo = prod.Costo,
                    Detalle = det.Detalle,
                    IdCarrito = det.IdCarrito,
                    Id = det.Id
                }).Where(det => det.IdCarrito == id).ToList();
            
            if (detalles == null)
            {
                return NotFound();
            }
            return Ok(detalles);
        }

        // PUT: api/DetalleCarrito/5
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/DetalleCarrito/ModifyDetalleCarrito/{id}/{accion}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult ModifyDetalleCarrito(int id, bool accion)
        {
            DetalleCarrito detalleCarrito = new DetalleCarrito();
            detalleCarrito = db.DetalleCarrito.Find(id);
            if(accion == true)
            {
                detalleCarrito.Cantidad += 1;
            } else
            {
                if (detalleCarrito.Cantidad > 1)
                {
                    detalleCarrito.Cantidad -= 1;
                }                
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
                    return StatusCode(HttpStatusCode.BadRequest);
                }
            }

            return StatusCode(HttpStatusCode.OK);
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

        
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/DetalleCarrito/DeleteDetalleCarrito/{id}")]        

        [ResponseType(typeof(DetalleCarrito))]
        public IHttpActionResult DeleteDetalleCarritoPedido(int id)
        {
            DetalleCarrito detalleCarrito = db.DetalleCarrito.FirstOrDefault(e => e.IdCarrito == id);
            if (detalleCarrito == null)
            {
                return NotFound();
            }

            db.DetalleCarrito.RemoveRange(db.DetalleCarrito.Where(d => d.IdCarrito == id));
            db.SaveChanges();

            return Ok();
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
