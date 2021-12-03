using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TienditaAPI.Models
{
    public class SesionModel
    {
        public Usuario usuario { get; set; }
        public string Token { get; set; }
    }
}