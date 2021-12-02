using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TienditaAPI.Models
{
    public class DetalleCarritoProductoModel
    {
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public double Costo { get; set; }
        public string Detalle { get; set; }
        public int IdCarrito { get; set; }
        public int Id { get; set; }

    }
}