using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TienditaAPI.Models
{
    public class AuthModel
    {
        public string Correo { get; set; }
        public string Contrasenia { get; set; }
    }
}