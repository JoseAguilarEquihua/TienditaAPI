using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TienditaAPI.Models
{
    public class PedidoUsuarioModel
    {
        public int IdPedido { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public double Total { get; set; }

    }
}