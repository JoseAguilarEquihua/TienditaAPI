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
    public class PedidoController : ApiController
    {
        private TienditaEntities1 db = new TienditaEntities1();

        // GET: api/Pedido
        public IQueryable<Pedido> GetPedido()
        {                        
            return db.Pedido;
        }

        [System.Web.Http.Route("api/PedidoController/GetPedidosProducto")]
        [ResponseType(typeof(PedidoUsuarioModel))]
        public IHttpActionResult GetPedidosProducto()
        {                       
            List<PedidoUsuarioModel> pedidosProducto = db.Pedido.Join(db.Usuario, ped => ped.Correo, usr => usr.Correo,
                (ped, usr) => new PedidoUsuarioModel
                {
                    IdPedido = ped.IdPedido,
                    Nombre = usr.Nombre,
                    Apellidos = usr.Apellidos,
                    Telefono = usr.Telefono,
                    Total = (double) ped.Total

                }).ToList();

            if (pedidosProducto == null)
            {
                return NotFound();
            }
            return Ok(pedidosProducto);
        }

        [System.Web.Http.Route("api/PedidoController/GetPedidosProducto/{id}")]
        [ResponseType(typeof(PedidoUsuarioModel))]
        public IHttpActionResult GetPedidosProductoUsuario(int id)
        {
            PedidoUsuarioModel pedidoUsuario = db.Pedido.Join(db.Usuario, ped => ped.Correo, usr => usr.Correo,
                (ped, usr) => new PedidoUsuarioModel
                {
                    IdPedido = ped.IdPedido,
                    Nombre = usr.Nombre,
                    Apellidos = usr.Apellidos,
                    Telefono = usr.Telefono,
                    Total = (double)ped.Total

                }).FirstOrDefault(det => det.IdPedido == id);

            if (pedidoUsuario == null)
            {
                return NotFound();
            }
            return Ok(pedidoUsuario);
        }

        [System.Web.Http.Route("api/PedidoController/GetDetallePedido/{id}")]
        [ResponseType(typeof(DetallePedidoUsuarioModel))]
        public IHttpActionResult GetDetallePedido(int id)
        {
            List<DetallePedidoUsuarioModel> detallePedidoUsuario = db.DetallePedido.Join(db.Producto, det => det.IdProducto, prod=> prod.IdProducto,
                (det, prod) => new DetallePedidoUsuarioModel
                {
                    IdPedido = det.IdPedido,
                    Producto = prod.Producto1,
                    Cantidad = det.Cantidad,
                    Costo = prod.Costo,
                    Detalle = det.Detalle,
                    Subtotal = prod.Costo * det.Cantidad
                }).Where(det => det.IdPedido == id).ToList();

            if (detallePedidoUsuario == null)
            {
                return NotFound();
            }
            return Ok(detallePedidoUsuario);
        }
        

        // GET: api/Pedido/5
        [ResponseType(typeof(Pedido))]
        public IHttpActionResult GetPedido(int id)
        {
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        // PUT: api/Pedido/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPedido(int id, Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pedido.IdPedido)
            {
                return BadRequest();
            }

            db.Entry(pedido).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
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

        // POST: api/Pedido
        [ResponseType(typeof(Pedido))]
        public IHttpActionResult PostPedido(Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pedido.Add(pedido);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pedido.IdPedido }, pedido);
        }

        // DELETE: api/Pedido/5
        [ResponseType(typeof(Pedido))]
        public IHttpActionResult DeletePedido(int id)
        {
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return NotFound();
            }

            db.Pedido.Remove(pedido);
            db.SaveChanges();

            return Ok(pedido);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PedidoExists(int id)
        {
            return db.Pedido.Count(e => e.IdPedido == id) > 0;
        }
    }
}