using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WALLY_PROJECT.Models
{
    public class CuentaUsuario
    {
        public Usuario Usuario { get; set; }
        public Cuenta Cuenta { get; set; }
    }
}