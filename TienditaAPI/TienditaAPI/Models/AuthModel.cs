using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TienditaAPI.Models
{
    public class AuthModel
    {
        [Key]
        public string Correo { get; set; }    
        public string Contrasenia { get; set; }
    }
}