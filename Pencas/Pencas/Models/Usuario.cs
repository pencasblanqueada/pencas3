using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pencas.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public int Puntaje { get; set; }
    }
}